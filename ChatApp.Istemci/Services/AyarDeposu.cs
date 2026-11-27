using System.Text.Json;
using ChatApp.Istemci.Models;

namespace ChatApp.Istemci.Services;

/// <summary>
/// İstemci ayarlarını %AppData%\YMH219-ChatApp\istemci-ayarlari.json dosyasında saklar.
/// Ayar dosyası önemli değil: okunamazsa varsayılan ayarlar, yazılamazsa sessizce geçilir.
/// </summary>
public class AyarDeposu
{
    public string DosyaYolu { get; }

    public AyarDeposu()
        : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                            "YMH219-ChatApp", "istemci-ayarlari.json"))
    {
    }

    public AyarDeposu(string dosyaYolu) => DosyaYolu = dosyaYolu;

    public IstemciAyarlari Oku()
    {
        try
        {
            if (File.Exists(DosyaYolu))
                return JsonSerializer.Deserialize<IstemciAyarlari>(File.ReadAllText(DosyaYolu)) ?? new IstemciAyarlari();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
        {
            // bozuk ya da erişilemeyen dosya: varsayılanlarla devam
        }
        return new IstemciAyarlari();
    }

    public void Kaydet(IstemciAyarlari ayarlar)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(DosyaYolu)!);
            File.WriteAllText(DosyaYolu, JsonSerializer.Serialize(ayarlar, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            // ayar kaydedilemedi; sohbeti etkilemez
        }
    }
}
