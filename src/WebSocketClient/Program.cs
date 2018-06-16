using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using TcpGameServer.Contracts;

namespace WebSocketClient
{
    class Program
    {
        private static HubConnection _connection;
        static async Task Main(string[] args)
        {

            await StartConnectionAsync();
            _connection.On<string>("direct", (message) =>
            {
                Console.WriteLine($"Server: {message}");
            });

            await RequestServerLogonAsync(_connection);
            await Task.Delay(10000); // temp until refactor of this client.
            await RequestNewGameAsync(_connection);
            await _connection.SendAsync("Send", "Hi server, can you log me in?");

            Console.ReadLine();
            await DisposeAsync();
        }

        private static Task RequestNewGameAsync(HubConnection client)
        {
            var newGameRequest = new ClientRequest
            {
                RequestName = "lobby.newgame",
            };
            return SendRequestAsync<ClientRequest>(client, newGameRequest);
        }

        private static Task RequestServerLogonAsync(HubConnection client)
        {
            var authRequest = new ClientRequest
            {
                RequestName = "Authenticate",
                Payload = "arch;1234"
            };

            return SendRequestAsync<ClientRequest>(client, authRequest);
        }

        private static Task SendRequestAsync<T>(HubConnection client, T request)
        {
            var json = JsonConvert.SerializeObject(request);
            return _connection.SendAsync("Send", json);
        }

        public static async Task StartConnectionAsync()
        {
            _connection = new HubConnectionBuilder()
                 .WithUrl("http://127.0.0.1:5000/ws")
                 .Build();

            await _connection.StartAsync();
        }

        public static async Task DisposeAsync()
        {
            await _connection.DisposeAsync();
        }
    }
}
