using ChatApp.Ortak.Ag;

namespace ChatApp.Sunucu.Models;

/// <summary>Sunucuya bağlı bir istemcinin temel bilgileri ve bağlantısı.</summary>
public class BagliIstemci
{
    public string KullaniciAdi { get; }

    /// <summary>İstemcinin IP adresi ve portu, ör. "127.0.0.1:53120".</summary>
    public string UzakAdres => Baglanti.UzakAdres;

    public DateTime BaglanmaZamani { get; }

    public MesajBaglantisi Baglanti { get; }

    public BagliIstemci(string kullaniciAdi, MesajBaglantisi baglanti)
    {
        KullaniciAdi = kullaniciAdi;
        Baglanti = baglanti;
        BaglanmaZamani = DateTime.Now;
    }

    public override string ToString() => $"{KullaniciAdi} ({UzakAdres})";
}
