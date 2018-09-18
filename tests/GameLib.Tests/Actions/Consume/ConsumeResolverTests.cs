using System;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using GameLib.Actions.Consume;
using GameLib.Entities;
using GameLib.Properties.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameLib.Tests.Actions.Consume
{
	[TestClass]
	public class ConsumeResolverTests
	{
		private Mock<ICreatureRegistry> _registry;
		private ConsumeResolver _resolver;
		private PotionFactory _potionFactory;
		private Player _player;

		[TestInitialize]
		public void Arrange()
		{
			var container = AutoMock.GetLoose();
			_registry = container.Mock<ICreatureRegistry>();
			_resolver = container.Create<ConsumeResolver>();
			_potionFactory = new PotionFactory();
			var maxHitpoints = 12;
			var playerId = "arbitraryId";
			_player = CreatePlayer(playerId, maxHitpoints);
		}

		[TestMethod]
		public async Task WhenPlayerConsumesHealingPotion_ThenHitPointsAreIncreased()
		{
			var healingPotion = _potionFactory.CreateByName("Small Healing Potion");
			var currentHitpoints = 2;
			_player.Statistics.DefenseScores.HitPoints.Current = currentHitpoints;
			_player.Inventory.Items.Add(healingPotion);
			var consumeAction = new ConsumeAction(_player.Id, healingPotion.Id);

			_registry.Setup(i => i.GetById(It.IsAny<string>())).Returns(_player);

			await _resolver.ResolveAsync(consumeAction);

			var expectedHitPoints = currentHitpoints + healingPotion.Amount;
			Assert.AreEqual(expectedHitPoints, _player.Statistics.DefenseScores.HitPoints.Current);
		}

		[TestMethod]
		public async Task WhenConsumingHealingPotionEffectExtendsMaxHitPoints_ThenHitPointsAreLimitedToMax()
		{
			var healingPotion = _potionFactory.CreateByName("Small Healing Potion");
			var currentHitpoints = _player.Statistics.DefenseScores.HitPoints.Max - healingPotion.Amount + 2;
			_player.Statistics.DefenseScores.HitPoints.Current = currentHitpoints;
			_player.Inventory.Items.Add(healingPotion);
			var consumeAction = new ConsumeAction(_player.Id, healingPotion.Id);

			_registry.Setup(i => i.GetById(It.IsAny<string>())).Returns(_player);

			await _resolver.ResolveAsync(consumeAction);

			var expectedHitPoints = _player.Statistics.DefenseScores.HitPoints.Max;
			Assert.AreEqual(expectedHitPoints, _player.Statistics.DefenseScores.HitPoints.Current);
		}

		[TestMethod]
		public async Task WhenSuccessfullyConsumingAPotion_ThenThatPotionIsRemovedFromInventory()
		{
			var healingPotion = _potionFactory.CreateByName("Small Healing Potion");
			_player.Inventory.Items.Add(healingPotion);
			var consumeAction = new ConsumeAction(_player.Id, healingPotion.Id);

			_registry.Setup(i => i.GetById(It.IsAny<string>())).Returns(_player);

			var inventoryCountBeforeConsumption = _player.Inventory.Items.Count;
			await _resolver.ResolveAsync(consumeAction);
			var inventoryCountAfterConsumption = _player.Inventory.Items.Count;

			Assert.AreEqual(inventoryCountBeforeConsumption - 1, inventoryCountAfterConsumption);
		}

		[TestMethod]
		public async Task WhenPlayerTriesToConsumePotionThatDoesNotExist_ThenArgumentExceptionIsThrown()
		{
			var nonExistingPotionId = "nonExistingId";

			var consumeAction = new ConsumeAction(_player.Id, nonExistingPotionId);
			_registry.Setup(i => i.GetById(It.IsAny<string>())).Returns(_player);

			await Assert.ThrowsExceptionAsync<ArgumentException>(() => _resolver.ResolveAsync(consumeAction));
		}

		[TestMethod]
		public async Task WhenCreatureIdIsNotFound_ThenArgumentExceptionIsThrown()
		{
			var creatureId = "arbitraryId";
			var potionId = "arbitraryPotionId";
			var consumeAction = new ConsumeAction(creatureId, potionId);

			_registry.Setup(i => i.GetById(It.IsAny<string>())).Returns(default(Creature));

			await Assert.ThrowsExceptionAsync<ArgumentException>(() => _resolver.ResolveAsync(consumeAction));
		}

		[TestMethod]
		public async Task WhenCreatureIsNotAPlayerObject_ThenArgumentExceptionIsThrown()
		{
			var creatureId = "arbitraryId";
			var potionId = "arbitraryPotionId";
			var consumeAction = new ConsumeAction(creatureId, potionId);

			_registry.Setup(i => i.GetById(It.IsAny<string>())).Returns(new Creature { Id = creatureId });

			await Assert.ThrowsExceptionAsync<ArgumentException>(() => _resolver.ResolveAsync(consumeAction));
		}

		private static Player CreatePlayer(string playerId, int maxHitPoints)
		{
			var DefenseScores = new DefenseScores { HitPoints = new HitPoints(maxHitPoints) };
			return new Player { Id = playerId, Statistics = new Statistics { DefenseScores = DefenseScores } };
		}
	}
}