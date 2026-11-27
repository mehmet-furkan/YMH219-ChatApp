namespace ChatApp.Istemci.Services;

/// <summary>
/// Sunucuya bağlanırken veya mesaj gönderirken oluşan, kullanıcıya gösterilecek hata.
/// Mesajı doğrudan ekranda gösterilebilecek Türkçe bir açıklamadır.
/// </summary>
public class BaglantiHatasiException : Exception
{
    public BaglantiHatasiException(string mesaj, Exception? icHata = null) : base(mesaj, icHata)
    {
    }
}
