

using System.Threading.Tasks;
using GameLib;
using GameLib.Messaging;

namespace WebSocketServer
{
	public interface IStartupTaskRunner
	{
		Task ExecuteAsync();
	}

	public class StartupTaskRunner : IStartupTaskRunner
	{
		private readonly IRecipientRegistry _recipientRegistry;
		private readonly IClientSender _clientSender;

		public StartupTaskRunner(IRecipientRegistry recipientRegistry, IClientSender clientSender)
		{
			_recipientRegistry = recipientRegistry;
			_clientSender = clientSender;
		}

		public Task ExecuteAsync()
		{
			CreateInitialIndentities();
			return Task.CompletedTask;
		}

		private void CreateInitialIndentities()
		{
			// Temp until CRC design issue is resolved between LobbyRouter and SocketServer (IClientSender)
			_recipientRegistry.Register(new ClientMessageProxy(_clientSender, "1"));
			_recipientRegistry.Register(new ClientMessageProxy(_clientSender, "2"));
		}
	}
}