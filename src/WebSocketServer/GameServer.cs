using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GameLib.Actions;
using GameLib.Actions.Movement;
using GameLib.Area;
using GameLib.Players;
using TcpGameServer.Contracts.Area;
using TcpGameServer.Contracts;
using System.Linq;
using System.Collections.Generic;

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
    private readonly IActionResolver _actionResolver;
    private readonly PlayArea _playArea;
    private readonly IPlayerRepository _playerRepository;
    private readonly IGameStateRepository _gameStateRepository;
    private readonly GameConfiguration _gameConfiguration;

    public GameServer(ISocketServer socketServer, IActionRepository actionRepository, IActionResolver actionResolver,
        PlayArea playArea, IPlayerRepository playerRepository, IGameStateRepository gameStateRepository, GameConfiguration gameConfiguration)
    {
      _socketServer = socketServer;
      _actionRepository = actionRepository;
      _actionResolver = actionResolver;
      _playArea = playArea;
      _playerRepository = playerRepository;
      _gameStateRepository = gameStateRepository;
      _gameConfiguration = gameConfiguration;
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
          nextGameTick = DateTime.UtcNow.AddMilliseconds(_gameConfiguration.GameTickMiliseconds) - diff;

          await _actionResolver.ResolveAsync().ConfigureAwait(false);
          var connections = _socketServer.ActiveConnections;
          var players = await _playerRepository.GetAllPlayersAsync().ConfigureAwait(false);
          var playerMap = players.ToDictionary(i => i.Id);
          var activePlayers = players.Where(i => i.GameState == GameState.InGame).ToList();
          foreach (var connection in connections)
          {
            var player = playerMap[connection.Id];
            if (player.GameState == GameState.InGame)
            {
              var response = TempCreateStatusResponse(connection, activePlayers);
              await _socketServer.SendAsync(connection.ConnectionId, response).ConfigureAwait(false);
            }
          }
        }
      }
    }

    private ServerResponse TempCreateStatusResponse(Connection connection, List<Player> activePlayers)
    {
      var player = activePlayers.Single(i => i.Id == connection.Id);
      var pos = player.Position;
      var map = _gameStateRepository.GetMapById(connection.Id);
      const int LightRadius = 8;
      map.ComputeFov(pos.X, pos.Y, LightRadius);
      var status = new GameStatus
      {
        X = pos.X,
        Y = pos.Y,
        Map = new TcpGameServer.Contracts.Area.Map { Width = map.Width, Height = map.Height, VisibleCells = map.GetVisibleCells() }
      };

      foreach (var otherPlayer in activePlayers.Where(i => i.Id != connection.Id))
      {
        foreach (var cell in status.Map.VisibleCells)
        {
          var p = otherPlayer.Position;
          if (cell.X == p.X && cell.Y == p.Y)
          {
            cell.Visitor = new Entity { Name = otherPlayer.Name, Type = "Player" };
          }
        }
      }

      var response = new ServerResponse
      {
        Type = nameof(GameStatus).ToLower(),
        Payload = JsonConvert.SerializeObject(status)
      };

      return response;
    }
  }
}