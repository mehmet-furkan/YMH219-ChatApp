using System.Net.Sockets;
using System.Text;
using ChatApp.Ortak.Models;

namespace ChatApp.Ortak.Ag;

/// <summary>
/// Bir TcpClient'ı sarmalar: NetworkStream üzerinde StreamReader/StreamWriter ile
/// mesajları satır satır okur ve yazar. Hem sunucu (her istemci için bir tane) hem istemci kullanır.
/// </summary>
public sealed class MesajBaglantisi : IDisposable
{
    private readonly TcpClient _tcp;
    private readonly StreamReader _okuyucu;
    private readonly StreamWriter _yazici;
    private readonly SemaphoreSlim _yazmaKilidi = new(1, 1);

    /// <summary>Karşı tarafın adresi, ör. "192.168.1.20:52114".</summary>
    public string UzakAdres { get; }

    public MesajBaglantisi(TcpClient tcp)
    {
        _tcp = tcp;
        UzakAdres = tcp.Client.RemoteEndPoint?.ToString() ?? "?";

        NetworkStream akis = tcp.GetStream();
        var utf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
        _okuyucu = new StreamReader(akis, utf8);
        _yazici = new StreamWriter(akis, utf8) { AutoFlush = true, NewLine = "\n" };
    }

    /// <summary>
    /// Mesajı gönderir. Aynı anda iki görev yazarsa satırlar karışmasın diye yazma işlemi kilitlenir.
    /// </summary>
    public async Task GonderAsync(Mesaj mesaj, CancellationToken iptal = default)
    {
        string satir = MesajProtokolu.Kodla(mesaj);
        await _yazmaKilidi.WaitAsync(iptal);
        try
        {
            await _yazici.WriteLineAsync(satir.AsMemory(), iptal);
        }
        finally
        {
            _yazmaKilidi.Release();
        }
    }

    /// <summary>
    /// Bir sonraki mesajı bekler. Karşı taraf bağlantıyı kapattıysa null döner.
    /// Geçersiz (çözülemeyen) satırlar atlanır.
    /// </summary>
    public async Task<Mesaj?> AlAsync(CancellationToken iptal = default)
    {
        while (true)
        {
            string? satir = await _okuyucu.ReadLineAsync(iptal);
            if (satir is null)
                return null; // akış bitti: bağlantı kapandı

            var mesaj = MesajProtokolu.Coz(satir);
            if (mesaj is not null)
                return mesaj;
        }
    }

    /// <summary>Bağlantıyı kapatır. Karşı taraf zaten kopmuşsa oluşan hatalar yok sayılır.</summary>
    public void Dispose()
    {
        try { _yazici.Dispose(); } catch (Exception ex) when (ex is IOException or ObjectDisposedException) { }
        try { _okuyucu.Dispose(); } catch (Exception ex) when (ex is IOException or ObjectDisposedException) { }
        _tcp.Dispose();
    }
}
