using System.Collections.Generic;
using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TcpGameServer.Actions;
using TcpGameServer.Actions.Movement;
using TcpGameServer.Contracts;
using TcpGameServer.Logging;

namespace TcpGameServer.Tests.Actions
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
        public void WhenClientRequestHasValidRoute_ThenInserterHandlesTheRequest()
        {
            var clientId = "arbitrary client id";
            var request = new ClientRequest
            {
                RequestName = RequestKey,
                Payload = "arbitrary payload"
            };

            _gameRouter.Route(clientId, request);
            _inserter.Verify(i => i.Resolve(clientId, request));
        }

        [TestMethod]
        public void WhenClientRequestHasInvalidRoute_ThenLoggerInsertsAWarning()
        {
            var clientId = "arbitrary client id";
            var request = new ClientRequest
            {
                RequestName = "some.invalid.route",
                Payload = "arbitrary payload"
            };

            _gameRouter.Route(clientId, request);
            _logger.Verify(i => i.Warn(It.IsAny<string>()));
        }

        [TestMethod]
        public void WhenClientRequestHasEmptyRoute_ThenLoggerInsertsAWarning()
        {
            var clientId = "arbitrary client id";
            var request = new ClientRequest
            {
                RequestName = string.Empty,
                Payload = "arbitrary payload"
            };

            _gameRouter.Route(clientId, request);
            _logger.Verify(i => i.Warn(It.IsAny<string>()));
        }
    }
}