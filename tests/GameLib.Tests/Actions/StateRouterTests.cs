using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameLib.Actions;
using TcpGameServer.Contracts;
using GameLib.Players;

namespace GameLib.Tests.Actions
{
    [TestClass]
    public class StateRouterTests
    {
        private Mock<IPlayerRepository> _playerRepository;
        private Mock<ILobbyRouter> _lobbyRouter;
        private Mock<IGameRouter> _gameRouter;
        private StateRequestRouter _stateRouter;

        [TestInitialize]
        public void GivenStateWithGameAndLobbyRouter()
        {
            var container = AutoMock.GetLoose();
            _lobbyRouter = container.Mock<ILobbyRouter>();
            _gameRouter = container.Mock<IGameRouter>();
            _playerRepository = container.Mock<IPlayerRepository>();
            _stateRouter = container.Create<StateRequestRouter>();
        }

        [TestMethod]
        public void WhenStateIsSetToLobby_ThenRequestsAreRoutedToLobbyRouter()
        {
            var request = new ClientRequest { RequestName = "arbitrary_command" };
            var clientId = "arbitrary_id";
            _playerRepository.Setup(i => i.GetById(It.IsAny<string>())).Returns(new Player { GameState = GameState.lobby });

            _stateRouter.Route(clientId, request);

            _lobbyRouter.Verify(i => i.Route(It.IsAny<string>(), It.IsAny<ClientRequest>()));
        }

        [TestMethod]
        public void WhenStateIsSetToGame_ThenRequestsAreRoutedToGameRouter()
        {
            var request = new ClientRequest { RequestName = "arbitrary_command" };
            var clientId = "arbitrary_id";
            _playerRepository.Setup(i => i.GetById(It.IsAny<string>())).Returns(new Player { GameState = GameState.InGame });

            _stateRouter.Route(clientId, request);

            _gameRouter.Verify(i => i.Route(It.IsAny<string>(), It.IsAny<ClientRequest>()));
        }
    }
}