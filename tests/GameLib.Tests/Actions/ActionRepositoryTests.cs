using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLib.Actions;

namespace GameLib.Tests.Actions
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
		public void WhenPushingOneActionIntoEmptyRepository_ThenRepositoryCountIsOne()
		{
			var action = new Action { OwnerId = "arbitrary" };

			_actionRepository.PushInto(action);
			var currentCount = _actionRepository.Count();

			Assert.AreEqual(1, currentCount);
		}

		[TestMethod]
		public void WhenPullingOutActions_ThenTheyAreRemovedFromRepository()
		{
			var expectedCount = _actionRepository.Count();

			_actionRepository.PushInto(new Action { OwnerId = "arbitrary" });
			_actionRepository.PullOut();
			var currentCount = _actionRepository.Count();

			Assert.AreEqual(expectedCount, currentCount);
		}

		[TestMethod]
		public void WhenPushingActionsConcurrently_ThenAllActionsAreInsertedEventually()
		{
			var concurrentActionCount = 500;
			var tasks = new Task[concurrentActionCount];
			var nextId = 0;
			for (var i = 0; i < concurrentActionCount; ++i)
			{
				tasks[i] = Task.Run(() =>
				{
					Interlocked.Add(ref nextId, 1);
					InsertOneArbitraryAction(nextId);
				});
			}

			Task.WaitAll(tasks);

			var actions = _actionRepository.PullOut();
			Assert.AreEqual(concurrentActionCount, actions.Count);
		}

		[TestMethod]
		public void WhenPullingOutActions_ThenClientsCanStillAddActionsConcurrently()
		{
			var concurrentActionCount = 500;
			var tasks = new Task[concurrentActionCount];
			var nextId = 0;
			var firstBatchCount = 0;
			for (var i = 0; i < concurrentActionCount; ++i)
			{
				tasks[i] = Task.Run(() =>
				{
					Interlocked.Add(ref nextId, 1);
					InsertOneArbitraryAction(nextId);

					if (nextId == 100)
					{
						var firstBatchOfActions = _actionRepository.PullOut();
						firstBatchCount = firstBatchOfActions.Count;
					}
				});
			}

			Task.WaitAll(tasks);

			var secondBatchOfActions = _actionRepository.PullOut();
			var secondBatchCount = secondBatchOfActions.Count;
			var totalActionCount = firstBatchCount + secondBatchCount;

			Assert.AreEqual(totalActionCount, concurrentActionCount);
			Assert.IsTrue(firstBatchCount > 0);
		}

		private void InsertOneArbitraryAction(int id)
		{
			var action = new Action { OwnerId = id.ToString() };
			_actionRepository.PushInto(action);
		}
	}
}