using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using GameLib.Users;
using GameLib;

namespace WebSocketServer
{

	public interface IConnectionRegistry
	{
		IClientProxy GetProxyById(string userId);
		void AddOrUpdateProxy(string userId, IClientProxy clientProxy);
	}

	public class ConnectionRegistry : IConnectionRegistry
	{
		private readonly ConcurrentRegistry<string, IClientProxy> _connectionRegistry;

		public ConnectionRegistry()
		{
			_connectionRegistry = new ConcurrentRegistry<string, IClientProxy>();
		}

		public IClientProxy GetProxyById(string userId)
		{
			return _connectionRegistry.Get(userId);
		}

		public void AddOrUpdateProxy(string userId, IClientProxy clientProxy)
		{
			_connectionRegistry.RegisterOrUpdate(userId, clientProxy);
		}
	}
}