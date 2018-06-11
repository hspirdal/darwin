using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TcpGameServer.Actions;
using TcpGameServer.Actions.Movement;

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

		public GameServer(ITcpServer tcpServer, IActionRepository actionRepository, IPositionRepository positionRepository, IActionResolver actionResolver)
		{
			_tcpServer = tcpServer;
			_actionRepository = actionRepository;
			_positionRepository = positionRepository;
			_actionResolver = actionResolver;
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
						var pos = await _positionRepository.GetByIdAsync(connection.Id);
						var json = JsonConvert.SerializeObject(pos);
						_tcpServer.TempSend(connection.Client.Id, json);
					}
				}
			}
		}
	}
}