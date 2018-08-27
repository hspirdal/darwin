using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Client.Contracts;
using GameLib;
using GameLib.Users;

namespace WebSocketServer
{
	public class SocketHub : Hub
	{
		private readonly IClientRegistry _clientRegistry;
		private readonly JsonSerializerSettings _jsonSerializerSettings;
		private readonly IAuthenticator _authenticator;
		private readonly ConcurrentRegistry<string, string> _connectionUserRegistry;

		public SocketHub(IClientRegistry clientRegistry, IAuthenticator authenticator, ConcurrentRegistry<string, string> connectionUserRegistry)
		{
			_clientRegistry = clientRegistry;
			_authenticator = authenticator;
			_connectionUserRegistry = connectionUserRegistry;

			_jsonSerializerSettings = new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore,
				MissingMemberHandling = MissingMemberHandling.Ignore
			};
		}

		public override Task OnConnectedAsync()
		{
			var connectionId = Context.ConnectionId;
			_connectionUserRegistry.RegisterOrUpdate(connectionId, string.Empty);
			Console.WriteLine($"{connectionId} connected.");

			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(System.Exception exception)
		{
			var connectionId = Context.ConnectionId;
			var userId = _connectionUserRegistry.Get(connectionId);
			if (!string.IsNullOrEmpty(userId))
			{
				_clientRegistry.Disconnect(userId);
			}
			_connectionUserRegistry.Remove(connectionId);
			Console.WriteLine($"{connectionId} disconnected.");

			return base.OnDisconnectedAsync(exception);
		}

		public async Task SendAsync(string message)
		{
			var clientRequest = JsonConvert.DeserializeObject<ClientRequest>(message, _jsonSerializerSettings);
			if (clientRequest == null || clientRequest.SessionId == Guid.Empty)
			{
				await RespondMalformedRequestAsync().ConfigureAwait(false);
				return;
			}

			Console.WriteLine($"Incoming request: Name: '{clientRequest.RequestName}'. SessionId: '{clientRequest.SessionId}' Payload: '{clientRequest.Payload}'");

			var connectionId = Context.ConnectionId;
			var proxyClient = Clients.Client(connectionId);
			var success = await _authenticator.AuthenticateAsync(clientRequest.UserId, clientRequest.SessionId).ConfigureAwait(false);
			if (success)
			{
				_connectionUserRegistry.RegisterOrUpdate(connectionId, clientRequest.UserId);

				await _clientRegistry.HandleClientMessageAsync(clientRequest, proxyClient).ConfigureAwait(false);
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