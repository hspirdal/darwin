

using System.Threading.Tasks;
using GameLib;
using GameLib.Identities;
using GameLib.Messaging;

namespace WebSocketServer
{
	public interface IStartupTaskRunner
	{
		Task ExecuteAsync();
	}

	public class StartupTaskRunner : IStartupTaskRunner
	{
		private readonly IIdentityRepository _identityRepository;
		private readonly IRecipientRegistry _recipientRegistry;
		private readonly IClientSender _clientSender;

		public StartupTaskRunner(IIdentityRepository identityRepository, IRecipientRegistry recipientRegistry, IClientSender clientSender)
		{
			_identityRepository = identityRepository;
			_recipientRegistry = recipientRegistry;
			_clientSender = clientSender;
		}

		public async Task ExecuteAsync()
		{
			await CreateInitialIndentitiesAsync().ConfigureAwait(false);
		}

		private async Task CreateInitialIndentitiesAsync()
		{
			await _identityRepository.AddOrUpdateAsync(new Identity { Id = "1", UserName = "arch", Password = "1234" }).ConfigureAwait(false);
			await _identityRepository.AddOrUpdateAsync(new Identity { Id = "2", UserName = "clip", Password = "1234" }).ConfigureAwait(false);

			// Temp until CRC design issue is resolved bwtween LobbyRouter and SocketServer (IClientSender)
			_recipientRegistry.Register(new ClientMessageProxy(_clientSender, "1"));
			_recipientRegistry.Register(new ClientMessageProxy(_clientSender, "2"));
		}
	}
}