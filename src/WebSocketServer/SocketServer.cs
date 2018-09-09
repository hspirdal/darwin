using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameLib.Actions;
using GameLib.Logging;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Client.Contracts;
using GameLib;

namespace WebSocketServer
{
	public interface IClientRegistry
	{
		void Disconnect(string userId);
		Task ExecuteActionRequestAsync(ClientRequest clientRequest, IClientProxy clientProxy);
		Task ExecuteQueryRequestAsync(ClientRequest clientRequest, IClientProxy clientProxy);
	}

	public interface ISocketServer
	{
		Task SendAsync(string userid, ServerResponse response);
		Task SendAsync(string userid, string channel, ServerResponse response);
	}

	public class SocketServer : ISocketServer, IClientRegistry, IClientSender
	{
		private readonly ILogger _logger;
		private readonly IStateRequestRouter _stateRequestRouter;
		private readonly IConnectionRegistry _connectionRegistry;
		private readonly IQueryResolver _queryResolver;

		public SocketServer(ILogger logger, IStateRequestRouter stateRequestRouter, IConnectionRegistry connectionRegistry, IQueryResolver queryResolver)
		{
			_queryResolver = queryResolver;
			_logger = logger;
			_stateRequestRouter = stateRequestRouter;
			_connectionRegistry = connectionRegistry;
		}

		public Task SendAsync(string userId, ServerResponse serverResponse)
		{
			return SendAsync(userId, "direct", serverResponse);
		}

		public async Task SendAsync(string userId, string channel, ServerResponse serverResponse)
		{
			try
			{
				var proxy = _connectionRegistry.GetProxyById(userId);
				if (proxy != null)
				{
					var json = JsonConvert.SerializeObject(serverResponse);
					await proxy.SendAsync(channel, json).ConfigureAwait(false);
				}
			}
			catch (Exception e)
			{
				_logger.Error(e);
			}
		}

		public async Task ExecuteActionRequestAsync(ClientRequest clientRequest, IClientProxy clientProxy)
		{
			try
			{
				_connectionRegistry.AddOrUpdateProxy(clientRequest.UserId, clientProxy);
				var response = await _stateRequestRouter.RouteAsync(clientRequest.UserId, clientRequest).ConfigureAwait(false);
				await SendAsync(clientRequest.UserId, response).ConfigureAwait(false);
			}
			catch (Exception e)
			{
				_logger.Error(e);
			}
		}

		public async Task ExecuteQueryRequestAsync(ClientRequest clientRequest, IClientProxy clientProxy)
		{
			try
			{
				_connectionRegistry.AddOrUpdateProxy(clientRequest.UserId, clientProxy);
				var response = await _queryResolver.ResolveAsync(clientRequest).ConfigureAwait(false);
				await SendAsync(clientRequest.UserId, "direct", response).ConfigureAwait(false);
			}
			catch (Exception e)
			{
				_logger.Error(e);
			}
		}

		public void Disconnect(string userId)
		{
			_connectionRegistry.AddOrUpdateProxy(userId, null);
		}
	}
}