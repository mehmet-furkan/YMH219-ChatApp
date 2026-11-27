using ChatApp.Ortak.Models;

namespace ChatApp.Sunucu.Services;

/// <summary>
/// Sunucudan geçen mesajları günlük metin dosyalarına ekler:
/// %AppData%\YMH219-ChatApp\gecmis\sohbet-2026-11-27.txt
/// Yazma hatası sohbeti durdurmaz; Kaydet false döner ve SonHata doldurulur.
/// </summary>
public class MesajGecmisi
{
    private readonly object _kilit = new();

    public string Klasor { get; }
    public string? SonHata { get; private set; }

    public MesajGecmisi()
        : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YMH219-ChatApp", "gecmis"))
    {
    }

    public MesajGecmisi(string klasor) => Klasor = klasor;

    /// <summary>Bugünün geçmiş dosyasının yolu.</summary>
    public string BugununDosyasi => Path.Combine(Klasor, $"sohbet-{DateTime.Now:yyyy-MM-dd}.txt");

    public bool Kaydet(Mesaj mesaj)
    {
        try
        {
            lock (_kilit) // iki istemcinin mesajı aynı anda yazılmaya çalışılırsa satırlar karışmasın
            {
                Directory.CreateDirectory(Klasor);
                using var yazici = new StreamWriter(BugununDosyasi, append: true);
                yazici.WriteLine(mesaj.ToString());
            }
            SonHata = null;
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            SonHata = ex.Message;
            return false;
        }
    }
}
