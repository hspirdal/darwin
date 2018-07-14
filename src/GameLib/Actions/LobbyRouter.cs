using System;
using TcpGameServer.Contracts;
using TcpGameServer.Contracts.Area;
using GameLib.Actions.Movement;
using GameLib.Area;

using GameLib.Players;
using System.Threading.Tasks;

namespace GameLib.Actions
{
	public interface ILobbyRouter : IRouter { }

	public class LobbyRouter : ILobbyRouter
	{
		private readonly IPlayerRepository _playerRepository;
		private readonly PlayArea _playArea;

		public LobbyRouter(IPlayerRepository playerRepository,
		PlayArea playArea)
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
				var cell = GetRandomOpenCell();
				var player = _playerRepository.GetById(clientId);
				player.Position.SetPosition(cell.X, cell.Y);
				await _playerRepository.AddorUpdateAsync(player).ConfigureAwait(false);

				player.GameState = GameState.InGame;
				await _playerRepository.AddorUpdateAsync(player).ConfigureAwait(false);
			}
		}

		private TcpGameServer.Contracts.Area.Cell GetRandomOpenCell()
		{
			var openCells = _playArea.GameMap.GetAllWalkableCells();
			var random = new Random();
			var cellIndex = random.Next(openCells.Count - 1);
			var cell = openCells[cellIndex];
			return new TcpGameServer.Contracts.Area.Cell { X = cell.X, Y = cell.Y, IsWalkable = cell.IsWalkable };
		}
	}
}