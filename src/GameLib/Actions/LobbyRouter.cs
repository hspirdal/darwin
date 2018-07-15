using System;
using GameLib.Actions.Movement;
using GameLib.Area;
using GameLib.Players;
using System.Threading.Tasks;
using TcpGameServer.Contracts;

namespace GameLib.Actions
{
	public interface ILobbyRouter : IRouter { }

	public class LobbyRouter : ILobbyRouter
	{
		private readonly IPlayerRepository _playerRepository;
		private readonly IPlayArea _playArea;

		public LobbyRouter(IPlayerRepository playerRepository,
		IPlayArea playArea)
		{
			_playerRepository = playerRepository;
			_playArea = playArea;
		}

		public async Task RouteAsync(string clientId, ClientRequest clientRequest)
		{
			// Temp until there are more actions here.
			if (clientRequest.RequestName == "lobby.newgame")
			{
				Console.WriteLine("Spawning player..");
				var cell = _playArea.GameMap.GetRandomOpenCell();
				var player = _playerRepository.GetById(clientId);
				player.Position.SetPosition(cell.X, cell.Y);
				player.GameState = GameState.InGame;
				await _playerRepository.AddorUpdateAsync(player).ConfigureAwait(false);
				_playArea.GameMap.Add(cell.X, cell.Y, player);
			}
		}
	}
}