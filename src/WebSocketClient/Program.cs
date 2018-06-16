using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using TcpGameServer.Contracts;
using TcpGameServer.Contracts.Area;

namespace WebSocketClient
{
    internal class Program
    {
        private static Dictionary<char, MovementAction> _actionKeyMap;
        private static HubConnection _connection;
        private static ConsoleRenderer _renderer;

        private static async Task StartConnectionAsync()
        {
            _connection = new HubConnectionBuilder()
                 .WithUrl("http://127.0.0.1:5000/ws")
                 .Build();

            await _connection.StartAsync();
        }

        private static async Task DisposeAsync()
        {
            await _connection.DisposeAsync();
        }

        static async Task Main(string[] args)
        {
            _renderer = new ConsoleRenderer();

            await StartConnectionAsync();
            _connection.On<string>("direct", (message) =>
            {
                ResolveResponse(message);
            });

            Console.WriteLine("Enter a message and press enter...");

            try
            {
                await RequestServerLogonAsync(_connection);
                await Task.Delay(10000); // temp until refactor of this client.
                await RequestNewGameAsync(_connection);

                InitializeActionKeyMap();
                Console.Clear();

                while (true)
                {
                    var input = Console.ReadKey();

                    if (input.Key == ConsoleKey.Q)
                    {
                        break;
                    }

                    if (_actionKeyMap.ContainsKey(input.KeyChar))
                    {
                        var action = _actionKeyMap[input.KeyChar];
                        var request = new ClientRequest
                        {
                            RequestName = "Action.Movement",
                            Payload = JsonConvert.SerializeObject(action)
                        };

                        await SendRequestAsync(_connection, request);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            await DisposeAsync();
        }

        private static void ResolveResponse(string jsonResponse)
        {
            var status = JsonConvert.DeserializeObject<StatusResponse>(jsonResponse);
            if (status != null)
            {
                _renderer.Render(status.Map, status.X, status.Y);
            }
        }

        private static Task RequestNewGameAsync(HubConnection client)
        {
            var newGameRequest = new ClientRequest { RequestName = "lobby.newgame" };
            return SendRequestAsync<ClientRequest>(client, newGameRequest);
        }

        private static Task RequestServerLogonAsync(HubConnection client)
        {
            var authRequest = new ClientRequest { RequestName = "Authenticate", Payload = "arch;1234" };
            return SendRequestAsync<ClientRequest>(client, authRequest);
        }

        private static Task SendRequestAsync<T>(HubConnection client, T request)
        {
            var json = JsonConvert.SerializeObject(request);
            return _connection.SendAsync("Send", json);
        }

        private static void InitializeActionKeyMap()
        {
            _actionKeyMap = new Dictionary<char, MovementAction>
                {
                    { 'w', new MovementAction { OwnerId = 1, Name = "Action.Movement", MovementDirection = MovementDirection.North } },
                    { 's', new MovementAction { OwnerId = 1, Name = "Action.Movement", MovementDirection = MovementDirection.South } },
                    { 'a', new MovementAction { OwnerId = 1, Name = "Action.Movement", MovementDirection = MovementDirection.West } },
                    { 'd', new MovementAction { OwnerId = 1, Name = "Action.Movement", MovementDirection = MovementDirection.East } },
                };
        }
    }
}
