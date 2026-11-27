using ChatApp.Ortak.Models;

namespace ChatApp.Istemci.Models;

/// <summary>Mesaj listesindeki bir satırın türü; her tür farklı renkte çizilir.</summary>
public enum SatirTuru
{
    Diger,   // başkasının oda mesajı
    Kendi,   // benim gönderdiğim mesaj
    Ozel,    // /w ile gelen ya da giden özel mesaj
    Sistem,  // sunucu bildirimi (katıldı, ayrıldı ...)
    Hata     // bağlantı hatası, bağlantı koptu
}

/// <summary>Mesaj listesinde gösterilen tek satır: metin + nasıl renklendirileceği.</summary>
public class EkranSatiri
{
    public string Metin { get; }
    public SatirTuru Tur { get; }

    public EkranSatiri(string metin, SatirTuru tur)
    {
        Metin = metin;
        Tur = tur;
    }

    /// <summary>Sunucudan gelen mesajı, kimin gönderdiğine bakarak doğru türde satıra çevirir.</summary>
    public static EkranSatiri MesajdanOlustur(Mesaj mesaj, string benimAdim)
    {
        var tur = mesaj.Tur switch
        {
            MesajTuru.Ozel => SatirTuru.Ozel,
            MesajTuru.Sistem => SatirTuru.Sistem,
            _ when mesaj.Gonderen == benimAdim => SatirTuru.Kendi,
            _ => SatirTuru.Diger
        };
        return new EkranSatiri(mesaj.ToString(), tur);
    }

    public static EkranSatiri HataSatiri(string metin) =>
        new($"[{DateTime.Now:HH:mm:ss}] ! {metin.Replace('\n', ' ')}", SatirTuru.Hata);

    // ListBox ve ekran okuyucular satırın metnini ToString'den alır.
    public override string ToString() => Metin;
}
