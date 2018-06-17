using System.Collections.Generic;
using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameLib.Actions;
using GameLib.Actions.Movement;
using TcpGameServer.Contracts;
using GameLib.Logging;
using System.Threading.Tasks;

namespace GameLib.Tests.Actions
{
    [TestClass]
    public class GameRouterTests
    {
        private GameRouter _gameRouter;
        private Mock<IRequestInserter> _inserter;
        private Mock<ILogger> _logger;
        private const string RequestKey = "action.arbitrary.action.code";

        [TestInitialize]
        public void GivenGameRouterWithMovementRequestInserter()
        {
            var container = AutoMock.GetLoose();

            _inserter = container.Mock<IRequestInserter>();
            _logger = container.Mock<ILogger>();

            var inserterMap = new Dictionary<string, IRequestInserter>
            {
                { RequestKey, _inserter.Object }
            };

            _gameRouter = new GameRouter(_logger.Object, inserterMap);
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
            _inserter.Verify(i => i.Resolve(clientId, request));
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
        public async Task WhenClientRequestHasEmptyRoute_ThenLoggerInsertsAWarning()
        {
            var clientId = "arbitrary client id";
            var request = new ClientRequest
            {
                RequestName = string.Empty,
                Payload = "arbitrary payload"
            };

            await _gameRouter.RouteAsync(clientId, request);
            _logger.Verify(i => i.Warn(It.IsAny<string>()));
        }
    }
}