using GameLib.Entities;
using GameLib.Properties;
using System;
using System.Threading.Tasks;
using Client.Contracts;
using GameLib.Identities;

namespace GameLib.Actions
{
	public interface IStateRequestRouter : IRouter { }

	public class StateRequestRouter : IStateRequestRouter
	{
		private readonly ILobbyRouter _lobbyRouter;
		private readonly IGameRouter _gameRouter;
		private readonly IIdentityRepository _identityRepository;

		public StateRequestRouter(ILobbyRouter lobbyRouter, IGameRouter gameRouter, IIdentityRepository identityRepository)
		{
			_lobbyRouter = lobbyRouter;
			_gameRouter = gameRouter;
			_identityRepository = identityRepository;
		}

		public async Task RouteAsync(string clientId, ClientRequest clientRequest)
		{
			var player = await _identityRepository.GetByIdAsync(clientId).ConfigureAwait(false);
			if (player.GameState == GameState.lobby)
			{
				await _lobbyRouter.RouteAsync(clientId, clientRequest);
			}
			else
			{
				await _gameRouter.RouteAsync(clientId, clientRequest);
			}
		}
	}
}