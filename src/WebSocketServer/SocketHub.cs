using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace WebSocketServer
{
    public class SocketHub : Hub
    {
        private readonly IClientRegistry _clientRegistry;

        public SocketHub(IClientRegistry clientRegistry)
        {
            _clientRegistry = clientRegistry;
        }

        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            var clientProxy = Clients.Client(connectionId);
            _clientRegistry.Add(connectionId, clientProxy);

            return base.OnConnectedAsync();
        }
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.SendAsync("broadcastMessage", name, message);
            Console.WriteLine(Context.ConnectionId);
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            var connectionId = Context.ConnectionId;
            _clientRegistry.Remove(connectionId);

            return base.OnDisconnectedAsync(exception);
        }
    }
}