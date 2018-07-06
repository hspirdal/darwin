using System;
using Microsoft.AspNetCore.SignalR;

namespace WebSocketServer
{
    public class Connection
    {
        public string Id { get; set; }
        public string ConnectionId { get; set; }
        public Guid SessionId { get; set; }
        public IClientProxy Client { get; set; }
    }
}