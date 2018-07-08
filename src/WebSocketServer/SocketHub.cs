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
    private readonly JsonSerializerSettings _jsonSerializerSettings;

    public SocketHub(IClientRegistry clientRegistry)
    {
      _clientRegistry = clientRegistry;

      _jsonSerializerSettings = new JsonSerializerSettings
      {
        NullValueHandling = NullValueHandling.Ignore,
        MissingMemberHandling = MissingMemberHandling.Ignore
      };
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
      var clientRequest = JsonConvert.DeserializeObject<ClientRequest>(message, _jsonSerializerSettings);
      if (clientRequest == null || clientRequest.SessionId == Guid.Empty)
      {
        await RespondMalformedRequestAsync().ConfigureAwait(false);
      }

      Console.WriteLine($"Incoming request: Name: '{clientRequest.RequestName}'. SessionId: '{clientRequest.SessionId}' Payload: '{clientRequest.Payload}'");

      var proxyClient = Clients.Client(Context.ConnectionId);
      var success = await _clientRegistry.CheckValidConnectionAsync(connectionId, clientRequest.SessionId, proxyClient).ConfigureAwait(false);
      if (success)
      {
        if (clientRequest.RequestName != "Connection.Refresh")
        {
          await _clientRegistry.HandleClientMessageAsync(connectionId, clientRequest).ConfigureAwait(false);
        }
        await RespondRequestAcceptedAsync().ConfigureAwait(false);
      }
      else
      {
        await RespondNotAuthenticatedAsync().ConfigureAwait(false);
      }
    }

    private Task RespondMalformedRequestAsync()
    {
      return SendClientResponseAsync("direct", new ServerResponse("Request malformed"));
    }

    private Task RespondNotAuthenticatedAsync()
    {
      return SendClientResponseAsync("direct", new ServerResponse("Not authenticated") { Type = "NotAuthenticated" });
    }

    private Task RespondRequestAcceptedAsync()
    {
      return SendClientResponseAsync("direct", new ServerResponse("Request accepted"));
    }

    private Task SendClientResponseAsync(string channel, ServerResponse response)
    {
      var json = JsonConvert.SerializeObject(response);
      return Clients.Caller.SendAsync(channel, json);
    }
  }
}