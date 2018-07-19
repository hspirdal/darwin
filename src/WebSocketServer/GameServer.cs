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
		private readonly IPlayArea _playArea;
		private readonly IPlayerRepository _playerRepository;
		private readonly GameConfiguration _gameConfiguration;
		private readonly IMapper _mapper;

		public GameServer(ISocketServer socketServer, IActionRepository actionRepository, IActionResolver actionResolver,
				IPlayArea playArea, IPlayerRepository playerRepository, GameConfiguration gameConfiguration, IMapper mapper
		)
		{
			_socketServer = socketServer;
			_actionRepository = actionRepository;
			_actionResolver = actionResolver;
			_playArea = playArea;
			_playerRepository = playerRepository;
			_gameConfiguration = gameConfiguration;
			_mapper = mapper;
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
				Map = new Client.Contracts.Area.Map { Width = map.Width, Height = map.Height, VisibleCells = visibleCells }
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