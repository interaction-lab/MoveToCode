using System.Net;
using System.Net.Sockets;

namespace MoveToCode {
    public class UserIPAddr : Singleton<UserIPAddr> {
        public string GetLocalIPAddress() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork || ip.AddressFamily == AddressFamily.InterNetworkV6) {
                    return ip.ToString();
                }
            }
            return "NOLOCALIPFOUND";
        }

        public string GetGlobalIPAddress() {
            try {
                return new WebClient().DownloadString("http://icanhazip.com").Replace("\n", "");
            } 
            catch (WebException) {
                return "NOGLOBALIPFOUND";
            }
            return "NOGLOBALANDNOEXPECTION";
        }
    }
}