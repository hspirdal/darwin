using System;
using System.Threading.Tasks;
using TcpGameServer.Actions;

namespace TcpGameServer
{
	public class GameServer
	{
		private readonly ITcpServer _tcpServer;
		private readonly IActionRepository _actionRepository;
		private readonly IActionResolver _actionResolver;

		public GameServer(ITcpServer tcpServer, IActionRepository actionRepository, IActionResolver actionResolver)
		{
			_tcpServer = tcpServer;
			_actionRepository = actionRepository;
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

					await _actionRepository.SetNextResolveTimeAsync(nextGameTick).ConfigureAwait(false);
					await _actionResolver.ResolveAsync().ConfigureAwait(false);
					await _tcpServer.BroadcastAsync("Actions resolved. New round starting NOW...").ConfigureAwait(false);
				}
			}
		}
	}
}