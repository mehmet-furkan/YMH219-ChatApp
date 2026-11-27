namespace ChatApp.Ortak.Models;

/// <summary>Sunucu ile istemci arasında gidip gelen mesajların türü.</summary>
public enum MesajTuru
{
    Giris,            // istemci → sunucu: "bu kullanıcı adıyla bağlanıyorum"
    GirisKabul,       // sunucu → istemci: giriş başarılı
    GirisRed,         // sunucu → istemci: giriş reddedildi (ör. kullanıcı adı alınmış), Metin = sebep
    Sohbet,           // herkese açık oda mesajı
    Ozel,             // tek bir kullanıcıya özel mesaj (/w)
    Sistem,           // sunucunun bilgilendirme mesajı ("ali katıldı" vb.)
    KullaniciListesi, // sunucu → istemci: çevrimiçi kullanıcılar
    Cikis             // istemci → sunucu: bağlantıyı kapatıyorum
}
