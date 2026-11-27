using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ChatApp.Sunucu.Services;

/// <summary>Yerel ağdaki istemcilerin bağlanacağı IP adreslerini bulur.</summary>
public static class AgBilgisi
{
    /// <summary>Çalışan ağ bağdaştırıcılarının IPv4 adresleri (ör. 192.168.1.20).</summary>
    public static IReadOnlyList<string> YerelIPv4Adresleri()
    {
        try
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .Where(n => n.OperationalStatus == OperationalStatus.Up
                         && n.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .SelectMany(n => n.GetIPProperties().UnicastAddresses)
                .Where(a => a.Address.AddressFamily == AddressFamily.InterNetwork)
                .Select(a => a.Address.ToString())
                .Distinct()
                .ToList();
        }
        catch (NetworkInformationException)
        {
            return Array.Empty<string>();
        }
    }
}
