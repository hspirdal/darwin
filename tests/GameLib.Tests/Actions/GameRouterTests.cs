using System.Collections.Generic;
using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameLib.Actions;
using GameLib.Actions.Movement;
using Client.Contracts;
using GameLib.Logging;
using System.Threading.Tasks;
using System;

namespace GameLib.Tests.Actions
{
	[TestClass]
	public class GameRouterTests
	{
		private GameRouter _gameRouter;
		private Mock<IActionInserter> _inserter;
		private Mock<ILogger> _logger;
		private const string RequestKey = "Action.ArbitraryAction";

		[TestInitialize]
		public void GivenGameRouterWithValidInserters()
		{
			var container = AutoMock.GetLoose();

			_inserter = container.Mock<IActionInserter>();
			_logger = container.Mock<ILogger>();

			_gameRouter = new GameRouter(_logger.Object, _inserter.Object);
		}

		[TestMethod]
		public async Task WhenClientRequestHasValidRoute_ThenInserterHandlesTheRequest()
		{
			var clientId = "arbitrary client id";
			var request = new ClientRequest
			{
				RequestName = RequestKey,
				Payload = "arbitrary payload"
			};

			await _gameRouter.RouteAsync(clientId, request);
			_inserter.Verify(i => i.Insert(clientId, request.RequestName, request.Payload));
		}

		[TestMethod]
		public async Task WhenClientRequestHasInvalidRoute_ThenLoggerInsertsAWarning()
		{
			var clientId = "arbitrary client id";
			var request = new ClientRequest
			{
				RequestName = "some.invalid.route",
				Payload = "arbitrary payload"
			};

			await _gameRouter.RouteAsync(clientId, request);
			_logger.Verify(i => i.Warn(It.IsAny<string>()));
		}

		[TestMethod]
		public async Task WhenClientRequestHasEmptyRoute_ThenGameRouterThrowsArgumentException()
		{
			var clientId = "arbitrary client id";
			var request = new ClientRequest
			{
				RequestName = string.Empty,
				Payload = "arbitrary payload"
			};

			await Assert.ThrowsExceptionAsync<ArgumentException>(() => _gameRouter.RouteAsync(clientId, request));
		}
	}
}