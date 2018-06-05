using Ether.Network.Client;
using Ether.Network.Packets;
using System;
using System.Net.Sockets;

namespace TcpGameClient
{
    public class GameClient : NetClient
    {
        public GameClient(string host, int port, int bufferSize, int timeOut)
        {
            Configuration.Host = host;
            Configuration.Port = port;
            Configuration.BufferSize = bufferSize;
            //Configuration.TimeOut = timeOut;
        }

        public override void HandleMessage(INetPacketStream packet)
        {
            var response = packet.Read<string>();
            Console.WriteLine($"-> Server response: '{response}'");
        }

        protected override void OnConnected()
        {
            Console.WriteLine("Connected to {0}", this.Socket.RemoteEndPoint);
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine("Disconnected");
        }

        protected override void OnSocketError(SocketError socketError)
        {
            Console.WriteLine("Socket Error: {0}", socketError.ToString());
        }
    }
}