using System;
using Autofac.Extras.Moq;
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
			var container = AutoMock.GetLoose();
			_feedbackRepository = container.Create<FeedbackRepository>();
		}

		[TestMethod]
		public void WhenAddingValidMessage_ThenRepositoryMessageCountIsIncreasedByOne()
		{
			var preCount = _feedbackRepository.GetById("arbitraryId").Count;
			_feedbackRepository.Write("arbitraryId", "arbitraryCategory", "Some message");
			var postCount = _feedbackRepository.GetById("arbitraryId").Count;

			Assert.AreEqual(0, preCount);
			Assert.AreEqual(1, postCount);
		}

		[TestMethod]
		public void WhenClearingRepository_ThenMessageCountIsZero()
		{
			_feedbackRepository.Write("arbitraryId", "arbitraryCategory", "Some message");
			var preCount = _feedbackRepository.GetById("arbitraryId").Count;
			_feedbackRepository.Clear();
			var postCount = _feedbackRepository.GetById("arbitraryId").Count;

			Assert.AreEqual(1, preCount);
			Assert.AreEqual(0, postCount);
		}

		[TestMethod]
		public void WhenAddingMessageWithPlayerIdOfNullOrEmpty_ThenRepositoryThrowsArgumentException()
		{
			Assert.ThrowsException<ArgumentException>(() => _feedbackRepository.Write(string.Empty, "arbitraryCategory", "arbitrary message"));
		}
	}
}