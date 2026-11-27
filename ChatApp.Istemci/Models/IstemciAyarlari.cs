namespace ChatApp.Istemci.Models;

/// <summary>Son başarılı bağlantıda kullanılan bilgiler; bir sonraki açılışta alanlar bunlarla dolar.</summary>
public class IstemciAyarlari
{
    public string Sunucu { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 5050;
    public string KullaniciAdi { get; set; } = "";
}
