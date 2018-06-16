using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace WebSocketClient
{
    class Program
    {
        private static HubConnection _connection;
        static async Task Main(string[] args)
        {

            await StartConnectionAsync();
            _connection.On<string, string>("broadcastMessage", (name, message) =>
            {
                Console.WriteLine($"{name} said: {message}");
            });

            await _connection.SendAsync("Send", "arch", "asdasd");

            Console.ReadLine();
            await DisposeAsync();
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
