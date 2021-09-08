using CallOfCthulhuSheets.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json.Bson;
using System.IO;
using Newtonsoft.Json;

namespace CallOfCthulhuSheets.Services
{
    public class MessegeReceivedEventsArgs : EventArgs
    {
        public IPEndPoint IpEndPoint { get; set; }
        public byte[] Message { get; set; }
    }


    public class MultipleConnectionException : Exception { }


    public class LanUdpConnection
    {
        private Dictionary<Player, IPEndPoint> _connectedPlayers;
        private IPAddress _selfIPAddress;
        private readonly int _port;

        public bool IsReceiving { get; private set; }

        public IPAddress SelfIPAddress => _selfIPAddress;


        public LanUdpConnection(int port)
        {
            _ = GetSelfIPAddress();
            _connectedPlayers = new Dictionary<Player, IPEndPoint>();
            _port = port;
            IsReceiving = false;
        }


        private async Task GetSelfIPAddress()
        {

            if (_selfIPAddress == null)
            {
                if (!Connectivity.ConnectionProfiles.Contains(ConnectionProfile.Ethernet)
                    || Connectivity.ConnectionProfiles.Contains(ConnectionProfile.Cellular))
                    throw new MultipleConnectionException();

                var ips = await Dns.GetHostAddressesAsync(Dns.GetHostName());
                foreach (var address in ips)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        _selfIPAddress = address;
                    }
                }
            }

        }

        public event EventHandler<MessegeReceivedEventsArgs> MessageRecived;
        protected virtual void OnMessageRecived(MessegeReceivedEventsArgs e)
        {
            EventHandler<MessegeReceivedEventsArgs> handler = MessageRecived;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        public void StartAsyncReceiving()
        {
            if (IsReceiving)
                return;
            IsReceiving = true;
            _ = Task.Factory.StartNew(async () =>
            {
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, _port);
                using (var reciever = new UdpClient(iPEndPoint))
                {
                    try
                    {
                        reciever.EnableBroadcast = true;
                        while (true)
                        {
                            Debug.WriteLine("----!!! Start receiving");
                            var message = await reciever.ReceiveAsync();
                            if (!IsReceiving)
                                break;
                            if (message.RemoteEndPoint.Address.Equals(_selfIPAddress))
                                continue;
                            var args = new MessegeReceivedEventsArgs() { Message = message.Buffer, IpEndPoint = message.RemoteEndPoint };
                            OnMessageRecived(args);
                        }
                        reciever.Close();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        throw e;
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void StopReceiving()
        {
            IsReceiving = false;
        }



        private async Task SendBroadcastAsync(byte[] message)
        {
            try
            {
                int res = 0;
                var selfEndPoint = new IPEndPoint(_selfIPAddress, 0);
                using (var udpSender = new UdpClient(selfEndPoint))
                {
                    udpSender.Ttl = 3;
                    try
                    {
                        res = await udpSender.SendAsync(message, message.Length, new IPEndPoint(IPAddress.Broadcast, _port));
                    }
                    catch (Exception e) { Debug.WriteLine($"!---!!!:{e.Message}"); }
                }

                Debug.WriteLine($"!---!!!:{res}");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }


        public async Task SendBroadcastAsync<T>(T item) where T : class
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (var writer = new BsonWriter(ms))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(writer, item, item.GetType());
                }
                await SendBroadcastAsync(ms.ToArray());
            }
        }

    }
}
