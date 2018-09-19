using System.Threading.Tasks;
using Autofac.Extras.Moq;
using GameLib.Actions.Movement;
using GameLib.Area;
using GameLib.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameLib.Tests.Actions.Movement
{
	[TestClass]
	public class MovementResolverTests
	{
		[TestMethod]
		public async Task WhenMovingOneStepWest_ThenXPositionIsDecreasedByOne()
		{
			var container = AutoMock.GetLoose();
			var resolver = container.Create<MovementResolver>();
			var creature = new Creature { Id = "arbitraryId", Position = new GameLib.Properties.Position(5, 5) };

			var playArea = container.Mock<IPlayArea>();
			var creatureRegistry = container.Mock<ICreatureRegistry>();
			playArea.Setup(i => i.GameMap.IsWalkable(It.IsAny<int>(), It.IsAny<int>())).Returns(true);
			creatureRegistry.Setup(i => i.GetById(It.IsAny<string>())).Returns(creature);

			var action = new MovementAction("arbitraryId", MovementDirection.West);
			await resolver.ResolveAsync(action);

			Assert.AreEqual(4, creature.Position.X);
		}
	}
}