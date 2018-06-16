using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GameLib.Actions;
using GameLib.Actions.Movement;
using GameLib.Area;
using GameLib.Players;
using TcpGameServer.Contracts.Area;

namespace WebSocketServer
{
    public interface IGameServer
    {
        Task StartAsync();
    }

    public class GameServer : IGameServer
    {
        //private readonly ITcpServer _tcpServer;
        private readonly ISocketServer _messageServer;
        private readonly IActionRepository _actionRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IActionResolver _actionResolver;
        private readonly PlayArea _playArea;
        private readonly IPlayerRepository _playerRepository;

        public GameServer(ISocketServer messageServer, IActionRepository actionRepository, IPositionRepository positionRepository, IActionResolver actionResolver,
            PlayArea playArea, IPlayerRepository playerRepository)
        {
            //_tcpServer = tcpServer;
            _messageServer = messageServer;
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

                    await _messageServer.BroadcastAsync("tick tock");

                    // foreach (var connection in _tcpServer.GetConnections())
                    // {
                    //     var player = await _playerRepository.GetByIdAsync(connection.Id).ConfigureAwait(false);
                    //     if (player.GameState == GameState.InGame)
                    //     {
                    //         var json = await TempCreateStatusResponseAsync(connection);
                    //         //_tcpServer.TempSend(connection.Client.Id, json);
                    //     }
                    // }
                }
            }
        }

        // private async Task<string> TempCreateStatusResponseAsync(Connection connection)
        // {
        //     var pos = await _positionRepository.GetByIdAsync(connection.Id);
        //     var statusResponse = new StatusResponse
        //     {
        //         X = pos.X,
        //         Y = pos.Y,
        //         Map = _playArea.Map
        //     };
        //     var json = JsonConvert.SerializeObject(statusResponse);
        //     return json;
        // }
    }
}