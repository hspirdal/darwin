using GameLib.Entities;
using GameLib.Properties;
using System;
using System.Threading.Tasks;
using TcpGameServer.Contracts;

namespace GameLib.Actions
{
	public interface IStateRequestRouter : IRouter { }

	public class StateRequestRouter : IStateRequestRouter
	{
		private readonly ILobbyRouter _lobbyRouter;
		private readonly IGameRouter _gameRouter;
		private readonly IPlayerRepository _playerRepository;

		public StateRequestRouter(ILobbyRouter lobbyRouter, IGameRouter gameRouter, IPlayerRepository playerRepository)
		{
			_lobbyRouter = lobbyRouter;
			_gameRouter = gameRouter;
			_playerRepository = playerRepository;
		}

		public Task RouteAsync(string clientId, ClientRequest clientRequest)
		{
			var player = _playerRepository.GetById(clientId);
			if (player.GameState == GameState.lobby)
			{
				return _lobbyRouter.RouteAsync(clientId, clientRequest);
			}
			else
			{
				return _gameRouter.RouteAsync(clientId, clientRequest);
			}
		}
	}
}