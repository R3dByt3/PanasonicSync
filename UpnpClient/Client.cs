using Configuration.Contracts;
using DataStoring.Contracts;
using DataStoring.Contracts.UpnpResponse;
using NetStandard.Logger;
using Ninject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UpnpClient.Contracts;

namespace UpnpClient
{
    public class Client : IClient
    {
        private const string SearchString = "M-SEARCH * HTTP/1.1\r\nHOST:239.255.255.250:1900\r\nMAN:\"ssdp:discover\"\r\nST:urn:schemas-upnp-org:service:ContentDirectory:2\r\nMX:3\r\n\r\n";

        private readonly ILogger _logger;
        private readonly IKernel _kernel;
        private readonly ISettings _settings;

        private readonly IPEndPoint _localEndPoint;
        private readonly IPEndPoint _multicastEndPoint;

        public string LocalIP { get; private set; }

        public Client(IKernel kernel, ILoggerFactory loggerFactory)
        {
            _kernel = kernel;
            _logger = loggerFactory.CreateFileLogger();
            _settings = _kernel.Get<ISettings>();

            string ipaddress = GetAllLocalIPv4(NetworkInterfaceType.Ethernet & NetworkInterfaceType.Wireless80211).FirstOrDefault();
            LocalIP = ipaddress;
            _logger.Debug($"Machine IP = {ipaddress}");

            _multicastEndPoint = new IPEndPoint(IPAddress.Parse("239.255.255.250"), 1900);
            _localEndPoint = new IPEndPoint(IPAddress.Parse(ipaddress), 60000);
        }

        public string[] GetAllLocalIPv4(NetworkInterfaceType type)
        {
            List<string> ipAddrList = new List<string>();
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddrList.Add(ip.Address.ToString());
                        }
                    }
                }
            }
            return ipAddrList.ToArray();
        }

        public IEnumerable<IPanasonicDevice> SearchUpnpDevices()
        {
            using (Socket udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {

                udpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpSocket.Bind(_localEndPoint);
                udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(_multicastEndPoint.Address, IPAddress.Any));
                udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 2);
                udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastLoopback, true);

                _logger.Debug("UDP-Socket setup done...");

                udpSocket.SendTo(Encoding.UTF8.GetBytes(SearchString), SocketFlags.None, _multicastEndPoint);

                _logger.Debug("M-Search sent...");

                byte[] receiveBuffer = new byte[64000];

                int receivedBytes = 0;

                Stopwatch s = new Stopwatch();
                s.Start();
                while (s.Elapsed < TimeSpan.FromSeconds(_settings.DeviceDiscoveringTime))
                {
                    if (udpSocket.Available > 0)
                    {
                        receivedBytes = udpSocket.Receive(receiveBuffer, SocketFlags.None);

                        if (receivedBytes > 0)
                        {
                            var device = _kernel.Get<IPanasonicDevice>();
                            device.FillFromString(Encoding.UTF8.GetString(receiveBuffer, 0, receivedBytes));

                            _logger.Debug($"Found device: {device}");

                            yield return device;
                        }
                    }
                    else
                        Thread.Sleep(100);
                }
                s.Stop();
            }
        }
    }
}
