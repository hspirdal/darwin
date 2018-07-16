using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using GameLib.Actions;
using GameLib.Area;
using GameLib.Logging;
using GameLib.Entities;

namespace GameLib.Actions.Loot
{
	public class LootResolver : IResolver
	{
		private readonly ILogger _logger;
		private readonly IPlayerRepository _playerRepository;
		private readonly IPlayArea _playArea;
		public string ActionName => "action.loot";

		public LootResolver(ILogger logger, IPlayerRepository playerRepository, IPlayArea playArea)
		{
			_logger = logger;
			_playerRepository = playerRepository;
			_playArea = playArea;
		}

		public Task ResolveAsync(Action action)
		{
			// TODO: Improve design to avoid casting.
			var lootAction = (LootAction)action;
			return ResolveAsync(lootAction.OwnerId, lootAction.ItemId);
		}

		private async Task ResolveAsync(string playerId, string itemId)
		{
			var player = await _playerRepository.GetByIdAsync(playerId).ConfigureAwait(false);
			var cell = _playArea.GameMap.GetCell(player.Position.X, player.Position.Y);
			var item = cell.Content.Entities.SingleOrDefault(i => i.Type == "Item" && i.Id == itemId) as Item;
			if (item != null)
			{
				cell.Content.Entities.Remove(item);
				player.Inventory.Items.Add(item);
				_logger.Info($"Player '{player.Name}' looted item '{item.Name}'");
			}
			else
			{
				_logger.Warn($"Player '{player.Name}' failed to loot item with id '{itemId}'. It was either not found, or did not cast to item.");
			}
			await _playerRepository.AddorUpdateAsync(player).ConfigureAwait(false);
		}
	}
}