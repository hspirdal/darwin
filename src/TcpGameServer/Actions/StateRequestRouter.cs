using TcpGameServer.Contracts;
using TcpGameServer.Players;

namespace TcpGameServer.Actions
{
    public class StateRequestRouter : IRouter
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
            // TODO: change to string for id type
            var player = _playerRepository.GetById(int.Parse(clientId));
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