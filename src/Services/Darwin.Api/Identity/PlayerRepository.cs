using System.Collections.Generic;

namespace Darwin.Api.Identity
{
	public interface IPlayerRepository
	{
		Player GetById(int id);
	}

	public class PlayerRepository : IPlayerRepository
	{
		public readonly Dictionary<int, Player> _playerMap;

		public PlayerRepository(List<Player> initialPlayers)
		{
			_playerMap = new Dictionary<int, Player>();

			foreach (var player in initialPlayers)
			{
				_playerMap.Add(player.Id, player);
			}
		}

		public Player GetById(int id)
		{
			return _playerMap[id];
		}
	}
}