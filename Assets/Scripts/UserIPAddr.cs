using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace MoveToCode {
    public class UserIPAddr : Singleton<UserIPAddr> {
        public string GetLocalIPAddress() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList) {
                if ((ip.AddressFamily == AddressFamily.InterNetwork 
                    || ip.AddressFamily == AddressFamily.InterNetworkV6) 
                    && ip.ToString() != "127.0.0.1") { // Ignore local host
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
                // Throw up warning
                GameObject go = (Resources.FindObjectsOfTypeAll(typeof(NotConnectedToInternetPanelManager)) as NotConnectedToInternetPanelManager[])[0].gameObject;
                go.SetActive(true);
                return "NOGLOBALIPFOUND";
            }
            return "NOGLOBALANDNOEXPECTION";
        }
    }
}