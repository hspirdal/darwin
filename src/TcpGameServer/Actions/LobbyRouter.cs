using TcpGameServer.Contracts;
using TcpGameServer.Players;

namespace TcpGameServer.Actions
{
    public interface ILobbyRouter : IRouter { }

    public class LobbyRouter : ILobbyRouter
    {
        private readonly IPlayerRepository _playerRepository;

        public LobbyRouter(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public void Route(string clientId, ClientRequest clientRequest)
        {
            // Temp until there are more actions here.
            if (clientRequest.RequestName == "lobby.newgame")
            {
                var player = _playerRepository.GetById(clientId);
                player.GameState = GameState.InGame;
                _playerRepository.AddPlayerAsync(player);
            }
        }
    }
}