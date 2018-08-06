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
using GameLib.Properties.Autonomy;
using GameLib.Users;

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
		private readonly ICreatureRegistry _creatureRegistry;
		private readonly GameConfiguration _gameConfiguration;
		private readonly IMapper _mapper;
		private readonly ICooldownRegistry _cooldownRegistry;
		private readonly ICombatRegistry _combatRegistry;
		private readonly IAutonomousRegistry _autonomousRegistry;
		private readonly IUserRepository _userRepository;

		public GameServer(ILogger logger, ISocketServer socketServer, IActionRepository actionRepository, IActionResolver actionResolver,
				IPlayArea playArea, ICreatureRegistry creatureRegistry,
				GameConfiguration gameConfiguration, IMapper mapper, ICooldownRegistry cooldownRegistry, ICombatRegistry combatRegistry,
				IAutonomousRegistry autonomousRegistry, IUserRepository userRepository)
		{
			_logger = logger;
			_socketServer = socketServer;
			_actionRepository = actionRepository;
			_actionResolver = actionResolver;
			_playArea = playArea;
			_creatureRegistry = creatureRegistry;
			_gameConfiguration = gameConfiguration;
			_mapper = mapper;
			_cooldownRegistry = cooldownRegistry;
			_combatRegistry = combatRegistry;
			_autonomousRegistry = autonomousRegistry;
			_userRepository = userRepository;
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

					ResolveActionsForAutonomousEntities();

					await _actionResolver.ResolveAsync().ConfigureAwait(false);

					// TODO: Add user as part of the Connection class or the other way around..
					var activeUsers = await _userRepository.GetActiveAsync().ConfigureAwait(false);
					var connectionMap = _socketServer.ActiveConnections.ToDictionary(i => i.Id);

					foreach (var activeUser in activeUsers)
					{
						var connection = connectionMap[activeUser.Id];
						var player = _creatureRegistry.GetById(activeUser.Id) as Player;
						var response = TempCreateStatusResponse(connection, player);
						await _socketServer.SendAsync(connection.ConnectionId, response).ConfigureAwait(false);
					}
				}
			}
		}

		private void ResolveActionsForAutonomousEntities()
		{
			foreach (var autonomous in _autonomousRegistry.GetAll().Where(i => !_cooldownRegistry.IsCooldownInEffect(i.Id)))
			{
				autonomous.Act();
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
			var status = new GameStatus
			{
				Player = p,
				X = pos.X,
				Y = pos.Y,
				IsInCombat = _combatRegistry.IsInCombat(player.Id),
				Map = new Client.Contracts.Area.Map { Width = map.Width, Height = map.Height, VisibleCells = visibleCells },
				NextActionAvailableUtc = _cooldownRegistry.TimeUntilCooldownIsDone(player.Id)
			};

			// Temp, no point letting the client have to filter out self all the time...
			if (player.IsAlive)
			{
				var activeCell = visibleCells.Single(i => i.X == player.Position.X && i.Y == player.Position.Y);
				var playerInCell = activeCell.Creatures.Single(i => i.Id == player.Id);
				activeCell.Creatures.Remove(playerInCell);
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