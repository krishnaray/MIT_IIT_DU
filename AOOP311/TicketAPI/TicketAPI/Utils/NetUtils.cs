using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace Miths.Utils
{
    public static class NetUtils
    {
        public static IPAddress LocalIPAddress(IPv4InterfaceProperties intrfcProp = null)
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                if (intrfcProp != null)
                    socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, (int)IPAddress.HostToNetworkOrder(intrfcProp.Index));
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint? endPoint = socket.LocalEndPoint as IPEndPoint;
                socket.Close();
                return endPoint != null ? endPoint.Address : null;
            }
        }
        public static IPAddress LocalIPAddress() => LocalIPAddress(GetNetworkInterfaceType());
        public static IPAddress LocalIPAddress(NetworkInterface netInterface) {
            if (netInterface == null) return null;
            IPInterfaceProperties ipProperties = netInterface.GetIPProperties();
            if (ipProperties == null) return null;
            IPv4InterfaceProperties ipv4Properties = netInterface.GetIPProperties().GetIPv4Properties();
            if (ipv4Properties == null) return null;
            foreach (IPAddressInformation unicast in ipProperties.UnicastAddresses)
            {
                if (null == unicast) continue;
                if (unicast.Address.AddressFamily == AddressFamily.InterNetworkV6)
                    continue;
                return unicast.Address;
            }
            var ip = LocalIPAddress(ipv4Properties);
            return ip != null ? LocalIPAddress(ipv4Properties) :
                new IPEndPoint(IPAddress.HostToNetworkOrder(ipv4Properties.Index), 0).Address;
        }
        public static NetworkInterface GetNetworkInterfaceType(NetworkInterfaceType type = NetworkInterfaceType.Ethernet) {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics) {
                if (adapter == null) continue;
                IPInterfaceProperties ip_properties = adapter.GetIPProperties();
                if (ip_properties == null) continue;
                if (ip_properties.MulticastAddresses == null) continue;
                if (!ip_properties.MulticastAddresses.Any()) continue; // most of VPN adapters will be skipped
                if (!adapter.SupportsMulticast) continue; // multicast is meaningless for this type of connection
                if (OperationalStatus.Up != adapter.OperationalStatus) continue; // this adapter is off or not connected
                IPv4InterfaceProperties p = null;
                try { p = ip_properties.GetIPv4Properties(); }
                catch (Exception e) {}
                if (null == p) continue; // IPv4 is not configured on this adapter
                if (adapter.NetworkInterfaceType == type)
                    return adapter;
            }
            return null;
        }
    }
}
