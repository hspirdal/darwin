using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameLib.Identities;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using TcpGameServer.Contracts;

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
            Console.WriteLine($"{connectionId} connected.");

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            var connectionId = Context.ConnectionId;
            _clientRegistry.Remove(connectionId);
            Console.WriteLine($"{connectionId} disconnected.");

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendAsync(string message)
        {
            var connectionId = Context.ConnectionId;
            var clientRequest = JsonConvert.DeserializeObject<ClientRequest>(message);
            if (clientRequest == null)
            {
                await RespondMalformedRequestAsync().ConfigureAwait(false);
            }

            if (_clientRegistry.CheckValidConnection(connectionId, clientRequest.SessionId))
            {

                await _clientRegistry.HandleClientMessageAsync(connectionId, clientRequest).ConfigureAwait(false);
            }
            else
            {
                await TempAuthenticate(message).ConfigureAwait(false);
            }
        }

        private async Task TempAuthenticate(string json)
        {
            Console.WriteLine("begin auth: " + json);
            var clientRequest = JsonConvert.DeserializeObject<ClientRequest>(json);
            var connectionString = clientRequest.Payload;
            var kvp = connectionString.Split(';');
            var request = new AuthentificationRequest
            {
                UserName = kvp[0],
                Password = kvp[1],
                ConnectionId = Context.ConnectionId,
            };

            var proxyClient = Clients.Client(Context.ConnectionId);

            Console.WriteLine("Trying to auth..");
            var success = await _clientRegistry.AuthenticateAsync(request, proxyClient).ConfigureAwait(false);
            var msg = success ? "Authenticated successfully" : "Could not authenticate";
            Console.WriteLine(msg);
            var sessionId = _clientRegistry.GetSessionId(Context.ConnectionId);
            var response = new ServerResponse { Type = "string", Message = msg, Payload = $"SessionId: {sessionId}" };
            var serializedResponse = JsonConvert.SerializeObject(response);
            await proxyClient.SendAsync("direct", serializedResponse).ConfigureAwait(false);
        }

        private Task RespondMalformedRequestAsync()
        {
            return Clients.Caller.SendAsync("direct", "Malformed request");
        }
    }
}