using System;
using System.Linq;
using System.Threading.Tasks;
using GameLib.Entities;
using GameLib.Logging;

namespace GameLib.Actions.Consume
{
	public class ConsumeResolver : IResolver
	{
		private readonly ILogger _logger;
		private readonly ICreatureRegistry _creatureRegistry;

		public string ActionName => ConsumeAction.CanonicalName;

		public ConsumeResolver(ILogger logger, ICreatureRegistry creatureRegistry)
		{
			_logger = logger;
			_creatureRegistry = creatureRegistry;
		}

		public Task ResolveAsync(Action action)
		{
			// TODO: Improve design to avoid casting.
			var consumeAction = (ConsumeAction)action;
			Resolve(consumeAction.OwnerId, consumeAction.ItemId);
			return Task.CompletedTask;
		}

		private void Resolve(string creatureId, string itemId)
		{
			var player = GetPlayer(creatureId);
			var potion = player.Inventory.Items.SingleOrDefault(i => i.Id == itemId) as Potion;
			EnsureValidItem(creatureId, itemId, potion);

			if (potion.EffectTarget != EffectTarget.HitPoints)
			{
				throw new NotImplementedException("Only support for hit point effects at this time");
			}

			var hitPoints = player.Statistics.DefenseScores.HitPoints;
			player.Inventory.Items.Remove(potion);
			hitPoints.Current = Math.Min(hitPoints.Max, hitPoints.Current + potion.Amount);
		}

		private Player GetPlayer(string creatureId)
		{
			var creature = _creatureRegistry.GetById(creatureId);
			EnsureValidCreature(creatureId, creature);
			// Until creature base type gets it's own inventory slots, restrict to player only.
			var player = creature as Player;
			EnsureValidPlayer(player);
			return player;
		}

		private static void EnsureValidCreature(string creatureId, Creature creature)
		{
			if (creature == null)
			{
				throw new ArgumentException($"Creature with id '{creatureId}' was not found");
			}
		}

		private static void EnsureValidPlayer(Player player)
		{
			if (player == null)
			{
				throw new ArgumentException($"Only player objects are allowed to consume potions for now");
			}
		}

		private static void EnsureValidItem(string creatureId, string itemId, Potion potion)
		{
			if (potion == null)
			{
				throw new ArgumentException($"Item with id '{itemId}' was not found in inventory of player with id '{creatureId}'");
			}
		}
	}
}