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
        private readonly IGameStateRepository _gameStateRepository;
        private readonly PlayArea _playArea;

        public LobbyRouter(IPlayerRepository playerRepository, IPositionRepository positionRepository,
        IGameStateRepository gameStateRepository, PlayArea playArea)
        {
            _playerRepository = playerRepository;
            _positionRepository = positionRepository;
            _gameStateRepository = gameStateRepository;
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
                var playerMap = _playArea.GameMap.Clone();
                _gameStateRepository.SetMapById(player.Id, playerMap);

                player.GameState = GameState.InGame;
                await _playerRepository.AddPlayerAsync(player).ConfigureAwait(false);
            }
        }

        private Cell FindFirstOpenCell()
        {
            for (var y = 0; y < _playArea.GameMap.Height; ++y)
            {
                for (var x = 0; x < _playArea.GameMap.Width; ++x)
                {
                    var cell = _playArea.GameMap.GetCell(x, y);
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