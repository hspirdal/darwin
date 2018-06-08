using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TcpGameServer.Actions;

namespace TcpGameServer.Tests.Actions
{
	[TestClass]
	public class ActionRepositoryTests
	{
		private ActionRepository _actionRepository;

		[TestInitialize]
		public void Arrange()
		{
			_actionRepository = new ActionRepository();
		}

		[TestMethod]
		public void WhenAddingOneActionToEmptyRepository_ThenCountIsOne()
		{
			var action = new Action();

			_actionRepository.EnqueueAction(action);
			var actions = _actionRepository.DequeueActions();

			Assert.AreEqual(1, actions.Count);
		}



		[TestMethod]
		public void WhenAddingActionsConcurrently_ThenAllActionsAreInsertedCorrectly()
		{
			var threadCount = 500;
			var tasks = new Task[threadCount];
			var nextId = 0;
			for (var i = 0; i < threadCount; ++i)
			{
				tasks[i] = Task.Run(() =>
				{
					Interlocked.Add(ref nextId, 1);
					InsertOneArbitraryAction(nextId);
				});
			}

			Task.WaitAll(tasks);

			var actions = _actionRepository.DequeueActions();
			Assert.AreEqual(threadCount, actions.Count);
		}

		private void InsertOneArbitraryAction(int id)
		{
			var action = new Action { OwnerId = id };
			_actionRepository.EnqueueAction(action);
		}
	}
}