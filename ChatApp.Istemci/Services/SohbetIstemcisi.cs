using System.Net.Sockets;
using ChatApp.Ortak.Ag;
using ChatApp.Ortak.Models;

namespace ChatApp.Istemci.Services;

/// <summary>
/// Sunucuya TcpClient ile bağlanan sohbet istemcisi.
/// Bağlandıktan sonra arka planda gelen mesajları dinler ve olaylarla (event) bildirir.
/// Ağ hataları BaglantiHatasiException'a çevrilir; uygulama çökmez.
/// </summary>
public class SohbetIstemcisi
{
    private static readonly TimeSpan ZamanAsimi = TimeSpan.FromSeconds(5);

    private MesajBaglantisi? _baglanti;
    private CancellationTokenSource? _iptal;

    public string KullaniciAdi { get; private set; } = "";
    public bool Bagli => _baglanti is not null;

    /// <summary>Sohbet, özel veya sistem mesajı geldiğinde.</summary>
    public event EventHandler<Mesaj>? MesajAlindi;

    /// <summary>Sunucu çevrimiçi kullanıcı listesini gönderdiğinde.</summary>
    public event EventHandler<IReadOnlyList<string>>? KullaniciListesiGeldi;

    /// <summary>Bağlantı kapandığında (kullanıcı kesti, sunucu kapandı veya ağ koptu); argüman sebeptir.</summary>
    public event EventHandler<string>? BaglantiKesildi;

    /// <summary>
    /// Sunucuya bağlanır ve kullanıcı adıyla giriş yapar.
    /// Sunucu yoksa, yanıt vermezse veya kullanıcı adını reddederse BaglantiHatasiException fırlatır.
    /// </summary>
    public async Task BaglanAsync(string sunucu, int port, string kullaniciAdi)
    {
        if (Bagli)
            throw new InvalidOperationException("Zaten bağlısınız.");

        kullaniciAdi = kullaniciAdi.Trim();
        if (kullaniciAdi.Length == 0)
            throw new BaglantiHatasiException("Kullanıcı adı boş olamaz.");
        if (string.IsNullOrWhiteSpace(sunucu))
            throw new BaglantiHatasiException("Sunucu adresi boş olamaz.");

        var tcp = new TcpClient();
        using var zamanAsimi = new CancellationTokenSource(ZamanAsimi);
        try
        {
            await tcp.ConnectAsync(sunucu.Trim(), port, zamanAsimi.Token);
        }
        catch (OperationCanceledException)
        {
            tcp.Dispose();
            throw new BaglantiHatasiException($"Sunucu {ZamanAsimi.TotalSeconds:0} saniye içinde yanıt vermedi ({sunucu}:{port}).");
        }
        catch (SocketException ex)
        {
            tcp.Dispose();
            throw new BaglantiHatasiException(
                $"Sunucuya bağlanılamadı ({sunucu}:{port}).\nSunucu açık mı, IP ve port doğru mu?\n\nAyrıntı: {ex.Message}", ex);
        }

        var baglanti = new MesajBaglantisi(tcp);
        try
        {
            await baglanti.GonderAsync(Mesaj.Giris(kullaniciAdi), zamanAsimi.Token);
            var cevap = await baglanti.AlAsync(zamanAsimi.Token);

            if (cevap is null)
                throw new BaglantiHatasiException("Sunucu bağlantıyı kapattı.");
            if (cevap.Tur == MesajTuru.GirisRed)
                throw new BaglantiHatasiException("Sunucu girişi reddetti: " + cevap.Metin);
            if (cevap.Tur != MesajTuru.GirisKabul)
                throw new BaglantiHatasiException("Sunucudan beklenmeyen yanıt geldi.");
        }
        catch (Exception ex) when (ex is IOException or OperationCanceledException or BaglantiHatasiException)
        {
            baglanti.Dispose();
            if (ex is BaglantiHatasiException) throw;
            throw new BaglantiHatasiException("Sunucuyla giriş sırasında bağlantı koptu: " + ex.Message, ex);
        }

        _baglanti = baglanti;
        _iptal = new CancellationTokenSource();
        KullaniciAdi = kullaniciAdi;

        _ = DinleAsync(baglanti, _iptal.Token);
    }

    /// <summary>Yazılan metni sohbet odasına gönderir.</summary>
    public async Task GonderAsync(string metin)
    {
        if (_baglanti is null)
            throw new BaglantiHatasiException("Sunucuya bağlı değilsiniz.");

        metin = metin.Trim();
        if (metin.Length == 0)
            return;

        try
        {
            await _baglanti.GonderAsync(Mesaj.Sohbet(KullaniciAdi, metin));
        }
        catch (Exception ex) when (ex is IOException or ObjectDisposedException or SocketException)
        {
            throw new BaglantiHatasiException("Mesaj gönderilemedi, sunucu bağlantısı kopmuş olabilir.", ex);
        }
    }

    /// <summary>Sunucuya çıkış bildirir ve bağlantıyı kapatır. BaglantiKesildi olayı dinleme döngüsünden gelir.</summary>
    public async Task KesAsync()
    {
        var baglanti = _baglanti;
        if (baglanti is null)
            return;

        try
        {
            using var zamanAsimi = new CancellationTokenSource(TimeSpan.FromSeconds(1));
            await baglanti.GonderAsync(Mesaj.Cikis(KullaniciAdi), zamanAsimi.Token);
        }
        catch (Exception ex) when (ex is IOException or ObjectDisposedException or OperationCanceledException)
        {
            // sunucu zaten gitmişse sorun değil
        }

        _iptal?.Cancel();
        baglanti.Dispose();
    }

    /// <summary>Bağlantı açık kaldığı sürece gelen mesajları okur.</summary>
    private async Task DinleAsync(MesajBaglantisi baglanti, CancellationToken iptal)
    {
        string sebep = "Sunucu bağlantıyı kapattı.";
        try
        {
            while (true)
            {
                var mesaj = await baglanti.AlAsync(iptal);
                if (mesaj is null)
                    break;

                if (mesaj.Tur == MesajTuru.KullaniciListesi)
                    KullaniciListesiGeldi?.Invoke(this, mesaj.Kullanicilar ?? new List<string>());
                else
                    MesajAlindi?.Invoke(this, mesaj);
            }
        }
        catch (Exception ex) when (ex is OperationCanceledException or ObjectDisposedException)
        {
            sebep = "Bağlantı kesildi.";
        }
        catch (Exception ex) when (ex is IOException or SocketException)
        {
            sebep = iptal.IsCancellationRequested ? "Bağlantı kesildi." : "Sunucu ile bağlantı koptu.";
        }

        baglanti.Dispose();
        if (ReferenceEquals(_baglanti, baglanti))
            _baglanti = null;
        BaglantiKesildi?.Invoke(this, sebep);
    }
}
