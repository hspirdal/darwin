using TcpGameServer.Contracts;
using GameLib.Players;
using System;

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

        public void Route(string clientId, ClientRequest clientRequest)
        {
            var player = _playerRepository.GetById(clientId);
            if (player.GameState == GameState.lobby)
            {
                _lobbyRouter.Route(clientId, clientRequest);
            }
            else
            {
                _gameRouter.Route(clientId, clientRequest);
            }
        }
    }
}