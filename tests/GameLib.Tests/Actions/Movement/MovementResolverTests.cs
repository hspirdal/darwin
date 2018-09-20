using System.Threading.Tasks;
using Autofac.Extras.Moq;
using GameLib.Actions.Movement;
using GameLib.Area;
using GameLib.Combat;
using GameLib.Entities;
using GameLib.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameLib.Tests.Actions.Movement
{
	[TestClass]
	public class MovementResolverTests
	{
		private AutoMock _container;
		private MovementResolver _resolver;
		private Creature _creature;
		private Mock<IPlayArea> _playArea;
		private Mock<ICreatureRegistry> _creatureRegistry;

		[TestInitialize]
		public void Arrange()
		{
			_container = AutoMock.GetLoose();
			_resolver = _container.Create<MovementResolver>();
			_creature = new Creature { Id = "arbitraryId", Position = new Position(5, 5) };

			_playArea = _container.Mock<IPlayArea>();
			_creatureRegistry = _container.Mock<ICreatureRegistry>();
			_creatureRegistry.Setup(i => i.GetById(It.IsAny<string>())).Returns(_creature);
		}

		[TestMethod]
		public async Task WhenMovingOneStepWest_ThenXPositionIsDecreasedByOne()
		{
			_playArea.Setup(i => i.GameMap.IsWalkable(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
			var action = new MovementAction(_creature.Id, MovementDirection.West);

			await _resolver.ResolveAsync(action);

			Assert.AreEqual(4, _creature.Position.X);
		}

		[TestMethod]
		public async Task WhenMovingIntoUnwalkableTile_ThenMovementIsCancelled()
		{
			_playArea.Setup(i => i.GameMap.IsWalkable(It.IsAny<int>(), It.IsAny<int>())).Returns(false);
			var action = new MovementAction(_creature.Id, MovementDirection.West);

			await _resolver.ResolveAsync(action);

			Assert.AreEqual(5, _creature.Position.X);
		}

		[TestMethod]
		public async Task WhenMovingWhileInCombat_ThenMovementIsBlocked()
		{
			_playArea.Setup(i => i.GameMap.IsWalkable(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
			var combatRegistry = _container.Mock<ICombatRegistry>();
			combatRegistry.Setup(i => i.IsInCombat(It.IsAny<string>())).Returns(true);

			var action = new MovementAction(_creature.Id, MovementDirection.West);

			await _resolver.ResolveAsync(action);

			Assert.AreEqual(5, _creature.Position.X);
		}
	}
}