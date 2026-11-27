using System.Text.Json;
using System.Text.Json.Serialization;
using ChatApp.Ortak.Models;

namespace ChatApp.Ortak.Ag;

/// <summary>
/// Satır tabanlı JSON protokolü: her mesaj tek satırlık bir JSON metnidir, satır sonu (\n) mesajı bitirir.
/// TCP bir bayt akışı olduğu için mesajların nerede bittiğini bu sayede anlarız.
/// </summary>
public static class MesajProtokolu
{
    private static readonly JsonSerializerOptions Ayarlar = new()
    {
        // enum'lar sayı yerine "Sohbet", "Ozel" gibi okunur metin olarak yazılsın
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>Mesajı tek satırlık JSON'a çevirir (JSON içinde satır sonu kaçışlı yazılır, satır bölünmez).</summary>
    public static string Kodla(Mesaj mesaj) => JsonSerializer.Serialize(mesaj, Ayarlar);

    /// <summary>Bir satırı mesaja çevirir; satır geçerli bir mesaj değilse null döner.</summary>
    public static Mesaj? Coz(string satir)
    {
        if (string.IsNullOrWhiteSpace(satir))
            return null;

        try
        {
            return JsonSerializer.Deserialize<Mesaj>(satir, Ayarlar);
        }
        catch (JsonException)
        {
            return null; // bozuk satırı yok say, bağlantıyı düşürme
        }
    }
}
