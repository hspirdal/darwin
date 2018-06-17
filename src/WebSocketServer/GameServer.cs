using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GameLib.Actions;
using GameLib.Actions.Movement;
using GameLib.Area;
using GameLib.Players;
using TcpGameServer.Contracts.Area;
using TcpGameServer.Contracts;

namespace WebSocketServer
{
    public interface IGameServer
    {
        Task StartAsync();
    }

    public class GameServer : IGameServer
    {
        private readonly ISocketServer _socketServer;
        private readonly IActionRepository _actionRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IActionResolver _actionResolver;
        private readonly PlayArea _playArea;
        private readonly IPlayerRepository _playerRepository;

        public GameServer(ISocketServer socketServer, IActionRepository actionRepository, IPositionRepository positionRepository, IActionResolver actionResolver,
            PlayArea playArea, IPlayerRepository playerRepository)
        {
            _socketServer = socketServer;
            _actionRepository = actionRepository;
            _positionRepository = positionRepository;
            _actionResolver = actionResolver;
            _playArea = playArea;
            _playerRepository = playerRepository;
        }

        public async Task StartAsync()
        {
            // who needs precision or quickness >_>
            var nextGameTick = DateTime.UtcNow;
            while (true)
            {
                var currentTime = DateTime.UtcNow;
                if (currentTime > nextGameTick)
                {
                    var diff = currentTime - nextGameTick;
                    nextGameTick = DateTime.UtcNow.AddSeconds(1) - diff;

                    await _actionResolver.ResolveAsync().ConfigureAwait(false);

                    foreach (var connection in _socketServer.GetConnections())
                    {
                        var player = await _playerRepository.GetByIdAsync(connection.Id).ConfigureAwait(false);
                        if (player.GameState == GameState.InGame)
                        {
                            var response = await TempCreateStatusResponseAsync(connection).ConfigureAwait(false);
                            await _socketServer.SendAsync(connection.ConnectionId, response).ConfigureAwait(false);
                        }
                    }
                }
            }
        }

        private async Task<ServerResponse> TempCreateStatusResponseAsync(Connection connection)
        {
            var pos = await _positionRepository.GetByIdAsync(connection.Id).ConfigureAwait(false);
            var status = new GameStatus
            {
                X = pos.X,
                Y = pos.Y,
                Map = _playArea.Map
            };
            var response = new ServerResponse
            {
                ResponseType = nameof(GameStatus).ToLower(),
                Payload = JsonConvert.SerializeObject(status)
            };

            return response;
        }
    }
}