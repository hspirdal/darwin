using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace WebSocketServer
{
    public interface IClientRegistry
    {
        void Add(string connectionId, IClientProxy clientProxy);
        void Remove(string connectionId);
    }

    public interface ISocketServer
    {
        Task BroadcastAsync(string message);
    }

    public class SocketServer : ISocketServer, IClientRegistry
    {
        private readonly ConcurrentDictionary<string, IClientProxy> _connectionMap;

        public SocketServer()
        {
            _connectionMap = new ConcurrentDictionary<string, IClientProxy>();
        }

        public async Task BroadcastAsync(string message)
        {
            // TODO: only logged on users
            var clients = _connectionMap.Values;
            foreach (var client in clients)
            {
                await client.SendAsync("broadcastMessage", "Server", message);
            }
        }

        // public Task Send(string connectionId, string message)
        // {
        //     return _connectionMap[connectionId].SendAsync("directMessage", message);
        // }

        public void Add(string connectionId, IClientProxy clientProxy)
        {
            if (_connectionMap.ContainsKey(connectionId) == false)
            {
                _connectionMap.TryAdd(connectionId, clientProxy);
            }
        }

        public void Remove(string connectionId)
        {
            if (_connectionMap.ContainsKey(connectionId))
            {
                _connectionMap.TryRemove(connectionId, out IClientProxy clientProxy);
            }
        }
    }
}