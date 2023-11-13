using System.Net.Sockets;
using System.Net;

namespace Infrastructure.Helpers
{
    public class IpHelper
    {
        public static string GetApiAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }
    }
}
