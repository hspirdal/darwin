using GameLib.Actions;
using GameLib.Entities;
using GameLib.Logging;
using GameLib.Messaging;
using GameLib.Properties.Autonomy;
using GameLib.Properties.Stats;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameLib.Tests.Properties.Autonomy
{
	[TestClass]
	public class BasicDefenderTests
	{

		[TestMethod]
		public void WhenGettingAttacked_ThenAutonomousModelWillAttackBack()
		{
			var actionInserter = new Mock<IActionInserter>();
			var logger = new Logger();
			var creature = new Creature { Id = "arbitraryId" };
			var defender = new SimpleDefender(logger, actionInserter.Object, creature);

			var message = new GameMessage("attackerId", "arbitraryId", MessageTopic.AttackedBy);


			defender.Act(); // Initial idle state.
			defender.Receive(message); // Transition into combat and do one act.

			actionInserter.Verify(i => i.Insert(It.IsAny<Action>()), Times.Once);
		}
	}
}