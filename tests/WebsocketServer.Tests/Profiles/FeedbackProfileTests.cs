using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSocketServer.Profiles;

namespace WebsocketServer.Tests.Profiles
{
	[TestClass]
	public class FeedbackProfileTests
	{
		private MapperConfiguration _mapperConfiguration;

		[TestInitialize]
		public void GivenAValidFeedbackProfile()
		{
			_mapperConfiguration = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(typeof(FeedbackProfile));
			});
		}

		[TestMethod]
		public void WhenMappingPlayer_ThenMappingIsSuccessfull()
		{
			_mapperConfiguration.AssertConfigurationIsValid();
		}

		[TestMethod]
		public void WhenMappingFeedback_ThenFeedbackFieldsIsMappedCorrectly()
		{
			var feedback = new GameLib.Logging.Feedback { Category = "Category", Type = GameLib.Logging.FeedbackType.Success, Message = "Message" };
			var mapper = _mapperConfiguration.CreateMapper();

			var mappedPlayer = mapper.Map<GameLib.Logging.Feedback, Client.Contracts.Logging.Feedback>(feedback);

			Assert.AreEqual(feedback.Message, mappedPlayer.Message);
			Assert.AreEqual(feedback.Type.ToString(), mappedPlayer.Type.ToString());
			Assert.AreEqual(feedback.Category, mappedPlayer.Category);
		}
	}
}