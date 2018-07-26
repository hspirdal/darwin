using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using AutoMapper;
using GameLib.Actions;
using GameLib.Actions.Movement;
using GameLib.Area;
using GameLib.Entities;
using GameLib.Properties;
using Client.Contracts.Area;
using Client.Contracts;
using Client.Contracts.Properties;
using GameLib.Logging;
using GameLib.Combat;

namespace WebSocketServer
{
  public interface IGameServer
  {
    Task StartAsync();
  }

  public class GameServer : IGameServer
  {
    private readonly ILogger _logger;
    private readonly ISocketServer _socketServer;
    private readonly IActionRepository _actionRepository;
    private readonly IActionResolver _actionResolver;
    private readonly IPlayArea _playArea;
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly ICreatureRegistry _creatureRegistry;
    private readonly GameConfiguration _gameConfiguration;
    private readonly IMapper _mapper;
    private readonly ICooldownRegistry _cooldownRegistry;

    public GameServer(ILogger logger, ISocketServer socketServer, IActionRepository actionRepository, IActionResolver actionResolver,
        IPlayArea playArea, IFeedbackRepository feedbackRepository, ICreatureRegistry creatureRegistry,
        GameConfiguration gameConfiguration, IMapper mapper, ICooldownRegistry cooldownRegistry)
    {
      _logger = logger;
      _socketServer = socketServer;
      _actionRepository = actionRepository;
      _actionResolver = actionResolver;
      _playArea = playArea;
      _feedbackRepository = feedbackRepository;
      _creatureRegistry = creatureRegistry;
      _gameConfiguration = gameConfiguration;
      _mapper = mapper;
      _cooldownRegistry = cooldownRegistry;
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
          foreach (var connection in connections)
          {
            var player = _creatureRegistry.GetById(connection.Id) as Player;
            //if (player.GameState == GameState.InGame)
            //{
            var response = TempCreateStatusResponse(connection, player);
            await _socketServer.SendAsync(connection.ConnectionId, response).ConfigureAwait(false);
            //}
          }
          _feedbackRepository.Clear();
        }
      }
    }

    private ServerResponse TempCreateStatusResponse(Connection connection, Player player)
    {
      var pos = player.Position;
      var map = _playArea.GameMap;
      var lightRadius = player.Inventory.Items.FirstOrDefault(i => i.Name == "Torch") != null ? 8 : 2;
      map.ComputeFov(pos.X, pos.Y, lightRadius);
      var visibleCells = _mapper.Map<List<GameLib.Area.Cell>, List<Client.Contracts.Area.Cell>>(_playArea.GameMap.GetVisibleCells());
      var p = _mapper.Map<GameLib.Entities.Player, Client.Contracts.Entities.Player>(player);
      var feedback = _mapper.Map<List<GameLib.Logging.Feedback>, List<Client.Contracts.Logging.Feedback>>(_feedbackRepository.GetById(player.Id));
      var status = new GameStatus
      {
        Player = p,
        X = pos.X,
        Y = pos.Y,
        Map = new Client.Contracts.Area.Map { Width = map.Width, Height = map.Height, VisibleCells = visibleCells },
        Feedback = feedback,
        NextActionAvailableUtc = _cooldownRegistry.TimeUntilCooldownIsDone(player.Id)
      };

      // Temp, no point letting the client have to filter out self all the time...
      var activeCell = visibleCells.Single(i => i.X == player.Position.X && i.Y == player.Position.Y);
      var playerInCell = activeCell.Creatures.Single(i => i.Id == player.Id);
      activeCell.Creatures.Remove(playerInCell);

      var response = new ServerResponse
      {
        Type = nameof(GameStatus).ToLower(),
        Payload = JsonConvert.SerializeObject(status)
      };

      return response;
    }
  }
}