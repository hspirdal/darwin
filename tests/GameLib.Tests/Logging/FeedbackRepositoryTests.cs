using System;
using GameLib.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameLib.Tests.Logging
{
	[TestClass]
	public class FeedbackRepositoryTests
	{
		private FeedbackRepository _feedbackRepository;

		[TestInitialize]
		public void GivenAFeedbakRepository()
		{
			_feedbackRepository = new FeedbackRepository();
		}

		[TestMethod]
		public void WhenAddingValidMessage_ThenRepositoryMessageCountIsIncreasedByOne()
		{
			var preCount = _feedbackRepository.GetById("arbitraryId").Count;
			_feedbackRepository.WriteLine("arbitraryId", "Some message");
			var postCount = _feedbackRepository.GetById("arbitraryId").Count;

			Assert.AreEqual(0, preCount);
			Assert.AreEqual(1, postCount);
		}

		[TestMethod]
		public void WhenClearingRepository_ThenMessageCountIsZero()
		{
			_feedbackRepository.WriteLine("arbitraryId", "Some message");
			var preCount = _feedbackRepository.GetById("arbitraryId").Count;
			_feedbackRepository.Clear();
			var postCount = _feedbackRepository.GetById("arbitraryId").Count;

			Assert.AreEqual(1, preCount);
			Assert.AreEqual(0, postCount);
		}

		[TestMethod]
		public void WhenAddingMessageWithPlayerIdOfNullOrEmpty_ThenRepositoryThrowsArgumentException()
		{
			Assert.ThrowsException<ArgumentException>(() => _feedbackRepository.WriteLine(string.Empty, "arbitrary message"));
		}
	}
}