using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TcpGameServer.Actions;
using TcpGameServer.Actions.Movement;
using TcpGameServer.Area;
using TcpGameServer.Contracts.Area;

namespace TcpGameServer
{
	public interface IGameServer
	{
		Task StartAsync();
	}

	public class GameServer : IGameServer
	{
		private readonly ITcpServer _tcpServer;
		private readonly IActionRepository _actionRepository;
		private readonly IPositionRepository _positionRepository;
		private readonly IActionResolver _actionResolver;
		private readonly PlayArea _playArea;

		public GameServer(ITcpServer tcpServer, IActionRepository actionRepository, IPositionRepository positionRepository, IActionResolver actionResolver,
			PlayArea playArea)
		{
			_tcpServer = tcpServer;
			_actionRepository = actionRepository;
			_positionRepository = positionRepository;
			_actionResolver = actionResolver;
			_playArea = playArea;
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

					foreach (var connection in _tcpServer.GetConnections())
					{
						var json = await TempCreateStatusResponseAsync(connection);
						_tcpServer.TempSend(connection.Client.Id, json);
					}
				}
			}
		}

		private async Task<string> TempCreateStatusResponseAsync(Connection connection)
		{
			var pos = await _positionRepository.GetByIdAsync(connection.Id);
			var statusResponse = new StatusResponse
			{
				X = pos.X,
				Y = pos.Y,
				Map = _playArea.Map
			};
			var json = JsonConvert.SerializeObject(statusResponse);
			return json;
		}
	}
}