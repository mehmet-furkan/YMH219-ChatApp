namespace ChatApp.Ortak.Models;

/// <summary>
/// Ağ üzerinden gönderilen tek bir mesaj. Protokol her mesajı tek satırlık JSON'a çevirip gönderir.
/// Fabrika metotları (Sohbet, Ozel, Sistem ...) doğru alanları dolu bir mesaj oluşturmayı kolaylaştırır.
/// </summary>
public class Mesaj
{
    public MesajTuru Tur { get; set; }
    public string Gonderen { get; set; } = "";

    /// <summary>Sadece özel mesajlarda dolu: mesajın gideceği kullanıcı.</summary>
    public string? Alici { get; set; }

    public string Metin { get; set; } = "";
    public DateTime Zaman { get; set; } = DateTime.Now;

    /// <summary>Sadece KullaniciListesi mesajında dolu.</summary>
    public List<string>? Kullanicilar { get; set; }

    public static Mesaj Giris(string kullaniciAdi) => new() { Tur = MesajTuru.Giris, Gonderen = kullaniciAdi };
    public static Mesaj Cikis(string kullaniciAdi) => new() { Tur = MesajTuru.Cikis, Gonderen = kullaniciAdi };
    public static Mesaj Sohbet(string gonderen, string metin) => new() { Tur = MesajTuru.Sohbet, Gonderen = gonderen, Metin = metin };
    public static Mesaj Ozel(string gonderen, string alici, string metin) => new() { Tur = MesajTuru.Ozel, Gonderen = gonderen, Alici = alici, Metin = metin };
    public static Mesaj Sistem(string metin) => new() { Tur = MesajTuru.Sistem, Gonderen = "Sunucu", Metin = metin };
    public static Mesaj Liste(IEnumerable<string> kullanicilar) => new() { Tur = MesajTuru.KullaniciListesi, Kullanicilar = kullanicilar.ToList() };

    /// <summary>Ekranda ve log dosyasında gösterilecek okunur metin: "[14:05:31] ali: merhaba".</summary>
    public override string ToString() => Tur switch
    {
        MesajTuru.Sohbet => $"[{Zaman:HH:mm:ss}] {Gonderen}: {Metin}",
        MesajTuru.Ozel => $"[{Zaman:HH:mm:ss}] (özel) {Gonderen} → {Alici}: {Metin}",
        MesajTuru.Sistem => $"[{Zaman:HH:mm:ss}] * {Metin}",
        _ => $"[{Zaman:HH:mm:ss}] {Tur} {Gonderen} {Metin}".TrimEnd()
    };
}
