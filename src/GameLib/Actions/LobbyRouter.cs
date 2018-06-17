using System;
using TcpGameServer.Contracts;
using TcpGameServer.Contracts.Area;
using GameLib.Actions.Movement;
using GameLib.Area;

using GameLib.Players;
using System.Threading.Tasks;

namespace GameLib.Actions
{
    public interface ILobbyRouter : IRouter { }

    public class LobbyRouter : ILobbyRouter
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly PlayArea _playArea;

        public LobbyRouter(IPlayerRepository playerRepository, IPositionRepository positionRepository, PlayArea playArea)
        {
            _playerRepository = playerRepository;
            _positionRepository = positionRepository;
            _playArea = playArea;
        }

        public async Task RouteAsync(string clientId, ClientRequest clientRequest)
        {
            // Temp until there are more actions here.
            if (clientRequest.RequestName == "lobby.newgame")
            {
                var cell = FindFirstOpenCell();
                await _positionRepository.SetPositionAsync(clientId, cell.X, cell.Y).ConfigureAwait(false);

                var player = _playerRepository.GetById(clientId);
                player.GameState = GameState.InGame;
                await _playerRepository.AddPlayerAsync(player).ConfigureAwait(false);
            }
        }

        private Cell FindFirstOpenCell()
        {
            for (var y = 0; y < _playArea.Map.Height; ++y)
            {
                for (var x = 0; x < _playArea.Map.Width; ++x)
                {
                    var cell = _playArea.Map.GetCell(x, y);
                    if (cell.IsWalkable)
                    {
                        return cell;
                    }
                }
            }

            throw new ArgumentException("No cells are walkable.");
        }
    }
}