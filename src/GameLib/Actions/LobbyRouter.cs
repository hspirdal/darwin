using System;
using System.Threading.Tasks;
using GameLib.Area;
using GameLib.Entities;
using GameLib.Properties;
using Client.Contracts;
using GameLib.Identities;

namespace GameLib.Actions
{
	public interface ILobbyRouter : IRouter { }

	public class LobbyRouter : ILobbyRouter
	{
		private readonly IIdentityRepository _identityRepository;
		private readonly IPlayArea _playArea;

		public LobbyRouter(IIdentityRepository identityRepository, IPlayArea playArea)
		{
			_identityRepository = identityRepository;
			_playArea = playArea;
		}

		public async Task RouteAsync(string clientId, ClientRequest clientRequest)
		{
			// Temp until there are more actions here.
			if (clientRequest.RequestName == "lobby.newgame")
			{
				Console.WriteLine("Spawning player..");
				var player = await _identityRepository.GetByIdAsync(clientId).ConfigureAwait(false);
				player.GameState = GameState.InGame;
				await _identityRepository.AddOrUpdateAsync(player).ConfigureAwait(false);
			}
		}
	}
}