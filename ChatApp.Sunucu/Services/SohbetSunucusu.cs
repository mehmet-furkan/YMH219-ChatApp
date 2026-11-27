using System.Net;
using System.Net.Sockets;
using ChatApp.Ortak.Ag;
using ChatApp.Ortak.Models;
using ChatApp.Sunucu.Models;

namespace ChatApp.Sunucu.Services;

/// <summary>
/// TcpListener ile gelen bağlantıları kabul eden sohbet sunucusu.
/// Her istemci kendi async döngüsünde dinlenir; gelen sohbet mesajı odadaki herkese yayınlanır.
/// Arayüzden bağımsızdır: olup biteni olaylarla (event) bildirir.
/// </summary>
public class SohbetSunucusu
{
    private const int AzamiKullaniciAdiUzunlugu = 20;

    private readonly Dictionary<string, BagliIstemci> _istemciler = new(StringComparer.OrdinalIgnoreCase);
    private readonly object _kilit = new();
    private TcpListener? _dinleyici;
    private CancellationTokenSource? _iptal;
    private readonly MesajGecmisi? _gecmis;

    /// <param name="gecmis">Verilirse oda ve özel mesajlar bu dosyaya da kaydedilir.</param>
    public SohbetSunucusu(MesajGecmisi? gecmis = null) => _gecmis = gecmis;

    public bool Calisiyor => _dinleyici is not null;
    public int Port { get; private set; }

    public event EventHandler<BagliIstemci>? IstemciBaglandi;
    public event EventHandler<BagliIstemci>? IstemciAyrildi;

    /// <summary>Sunucu günlüğüne yazılacak her olay (bağlantı, mesaj, hata).</summary>
    public event EventHandler<string>? Gunluk;

    public IReadOnlyList<BagliIstemci> Istemciler
    {
        get { lock (_kilit) return _istemciler.Values.ToList(); }
    }

    /// <summary>
    /// Verilen portu dinlemeye başlar. Port kullanımdaysa SocketException fırlatır (form yakalayıp gösterir).
    /// </summary>
    public void Baslat(int port)
    {
        if (Calisiyor)
            throw new InvalidOperationException("Sunucu zaten çalışıyor.");

        var dinleyici = new TcpListener(IPAddress.Any, port);
        dinleyici.Start();

        _dinleyici = dinleyici;
        _iptal = new CancellationTokenSource();
        Port = port;
        GunlugeYaz($"Sunucu başlatıldı, port {port} dinleniyor.");
        if (_gecmis is not null)
            GunlugeYaz($"Mesaj geçmişi kaydediliyor: {_gecmis.BugununDosyasi}");

        _ = BaglantilariKabulEtAsync(dinleyici, _iptal.Token);
    }

    /// <summary>Dinlemeyi bırakır ve tüm istemci bağlantılarını kapatır.</summary>
    public void Durdur()
    {
        if (!Calisiyor) return;

        _iptal!.Cancel();
        _dinleyici!.Stop();
        _dinleyici = null;

        foreach (var istemci in Istemciler)
            istemci.Baglanti.Dispose(); // istemcinin okuma döngüsü hata alıp kendini temizler

        GunlugeYaz("Sunucu durduruldu.");
    }

    // ---------- Bağlantı kabulü ----------

    private async Task BaglantilariKabulEtAsync(TcpListener dinleyici, CancellationToken iptal)
    {
        while (!iptal.IsCancellationRequested)
        {
            TcpClient tcp;
            try
            {
                tcp = await dinleyici.AcceptTcpClientAsync(iptal);
            }
            catch (Exception ex) when (ex is OperationCanceledException or ObjectDisposedException or SocketException)
            {
                break; // Durdur() çağrıldı
            }

            _ = IstemciyiYonetAsync(new MesajBaglantisi(tcp), iptal);
        }
    }

    /// <summary>Tek bir istemcinin yaşam döngüsü: giriş → mesaj döngüsü → ayrılma.</summary>
    private async Task IstemciyiYonetAsync(MesajBaglantisi baglanti, CancellationToken iptal)
    {
        BagliIstemci? istemci = null;
        try
        {
            istemci = await GirisiKarsilaAsync(baglanti, iptal);
            if (istemci is null)
                return;

            while (true)
            {
                var mesaj = await baglanti.AlAsync(iptal);
                if (mesaj is null || mesaj.Tur == MesajTuru.Cikis)
                    break;

                await MesajiIsleAsync(istemci, mesaj);
            }
        }
        catch (Exception ex) when (ex is IOException or SocketException or ObjectDisposedException or OperationCanceledException)
        {
            // istemci aniden koptu veya sunucu durduruldu: aşağıda temizlenir
        }
        finally
        {
            baglanti.Dispose();
            if (istemci is not null)
                await IstemciyiCikarAsync(istemci, sunucuKapaniyor: iptal.IsCancellationRequested);
        }
    }

    /// <summary>İlk mesajın geçerli bir giriş olmasını bekler; kullanıcı adı alınmışsa reddeder.</summary>
    private async Task<BagliIstemci?> GirisiKarsilaAsync(MesajBaglantisi baglanti, CancellationToken iptal)
    {
        var giris = await baglanti.AlAsync(iptal);
        if (giris is null)
            return null;

        string ad = giris.Gonderen.Trim();
        string? hata = KullaniciAdiHatasi(giris.Tur, ad);

        BagliIstemci? istemci = null;
        if (hata is null)
        {
            lock (_kilit)
            {
                if (_istemciler.ContainsKey(ad))
                    hata = $"\"{ad}\" kullanıcı adı şu an kullanımda.";
                else
                    _istemciler[ad] = istemci = new BagliIstemci(ad, baglanti);
            }
        }

        if (istemci is null)
        {
            GunlugeYaz($"{baglanti.UzakAdres} girişi reddedildi: {hata}");
            await baglanti.GonderAsync(new Mesaj { Tur = MesajTuru.GirisRed, Metin = hata ?? "" }, iptal);
            return null;
        }

        await baglanti.GonderAsync(new Mesaj { Tur = MesajTuru.GirisKabul, Gonderen = ad }, iptal);
        GunlugeYaz($"{ad} bağlandı ({istemci.UzakAdres}).");
        IstemciBaglandi?.Invoke(this, istemci);

        await YayinlaAsync(Mesaj.Sistem($"{ad} sohbete katıldı."));
        await KullaniciListesiniGonderAsync();
        return istemci;
    }

    private static string? KullaniciAdiHatasi(MesajTuru tur, string ad)
    {
        if (tur != MesajTuru.Giris) return "İlk mesaj giriş olmalı.";
        if (ad.Length == 0) return "Kullanıcı adı boş olamaz.";
        if (ad.Length > AzamiKullaniciAdiUzunlugu) return $"Kullanıcı adı en fazla {AzamiKullaniciAdiUzunlugu} karakter olabilir.";
        if (ad.Contains(' ')) return "Kullanıcı adında boşluk olamaz.";
        return null;
    }

    private async Task IstemciyiCikarAsync(BagliIstemci istemci, bool sunucuKapaniyor)
    {
        lock (_kilit)
            _istemciler.Remove(istemci.KullaniciAdi);

        GunlugeYaz($"{istemci.KullaniciAdi} ayrıldı.");
        IstemciAyrildi?.Invoke(this, istemci);

        if (!sunucuKapaniyor)
        {
            await YayinlaAsync(Mesaj.Sistem($"{istemci.KullaniciAdi} sohbetten ayrıldı."));
            await KullaniciListesiniGonderAsync();
        }
    }

    // ---------- Mesajlar ----------

    private async Task MesajiIsleAsync(BagliIstemci gonderen, Mesaj gelen)
    {
        if (string.IsNullOrWhiteSpace(gelen.Metin))
            return;

        switch (gelen.Tur)
        {
            case MesajTuru.Sohbet:
                // Gönderen adını ve zamanı sunucu belirler: istemci başkası adına mesaj atamaz.
                var mesaj = Mesaj.Sohbet(gonderen.KullaniciAdi, gelen.Metin.Trim());
                GunlugeYaz(mesaj.ToString());
                await YayinlaAsync(mesaj);
                break;

            case MesajTuru.Ozel:
                await OzelMesajIletAsync(gonderen, gelen);
                break;
        }
    }

    /// <summary>/w ile gönderilen mesajı sadece alıcıya iletir, gönderene de bir kopyasını yollar.</summary>
    private async Task OzelMesajIletAsync(BagliIstemci gonderen, Mesaj gelen)
    {
        BagliIstemci? alici;
        lock (_kilit)
            _istemciler.TryGetValue(gelen.Alici?.Trim() ?? "", out alici);

        if (alici is null)
        {
            await GuvenliGonderAsync(gonderen, Mesaj.Sistem($"\"{gelen.Alici}\" adlı kullanıcı çevrimiçi değil."));
            return;
        }

        var mesaj = Mesaj.Ozel(gonderen.KullaniciAdi, alici.KullaniciAdi, gelen.Metin.Trim());
        GunlugeYaz(mesaj.ToString());
        GecmiseKaydet(mesaj);
        await GuvenliGonderAsync(alici, mesaj);
        if (alici != gonderen)
            await GuvenliGonderAsync(gonderen, mesaj);
    }

    /// <summary>Mesajı bağlı tüm istemcilere gönderir. Gönderilemeyen istemci kendi döngüsünde temizlenir.</summary>
    private async Task YayinlaAsync(Mesaj mesaj)
    {
        if (mesaj.Tur != MesajTuru.KullaniciListesi)
            GecmiseKaydet(mesaj);

        foreach (var istemci in Istemciler)
            await GuvenliGonderAsync(istemci, mesaj);
    }

    private Task KullaniciListesiniGonderAsync() =>
        YayinlaAsync(Mesaj.Liste(Istemciler.Select(i => i.KullaniciAdi).Order()));

    private static async Task<bool> GuvenliGonderAsync(BagliIstemci istemci, Mesaj mesaj)
    {
        try
        {
            await istemci.Baglanti.GonderAsync(mesaj);
            return true;
        }
        catch (Exception ex) when (ex is IOException or SocketException or ObjectDisposedException)
        {
            return false;
        }
    }

    private void GecmiseKaydet(Mesaj mesaj)
    {
        if (_gecmis is not null && !_gecmis.Kaydet(mesaj))
            GunlugeYaz("HATA: Mesaj geçmişi yazılamadı: " + _gecmis.SonHata);
    }

    private void GunlugeYaz(string metin) => Gunluk?.Invoke(this, metin);
}
