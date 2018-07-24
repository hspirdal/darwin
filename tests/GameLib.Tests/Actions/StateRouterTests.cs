using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameLib.Actions;
using Client.Contracts;
using System.Threading.Tasks;
using GameLib.Entities;
using GameLib.Properties;
using GameLib.Identities;

namespace GameLib.Tests.Actions
{
	[TestClass]
	public class StateRouterTests
	{
		private Mock<IIdentityRepository> _identityRepository;
		private Mock<ILobbyRouter> _lobbyRouter;
		private Mock<IGameRouter> _gameRouter;
		private StateRequestRouter _stateRouter;

		[TestInitialize]
		public void GivenStateWithGameAndLobbyRouter()
		{
			var container = AutoMock.GetLoose();
			_lobbyRouter = container.Mock<ILobbyRouter>();
			_gameRouter = container.Mock<IGameRouter>();
			_identityRepository = container.Mock<IIdentityRepository>();
			_stateRouter = container.Create<StateRequestRouter>();
		}

		[TestMethod]
		public async Task WhenStateIsSetToLobby_ThenRequestsAreRoutedToLobbyRouter()
		{
			var request = new ClientRequest { RequestName = "arbitrary_command" };
			var clientId = "arbitrary_id";
			_identityRepository.Setup(i => i.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new Identity { GameState = GameState.lobby });

			await _stateRouter.RouteAsync(clientId, request);

			_lobbyRouter.Verify(i => i.RouteAsync(It.IsAny<string>(), It.IsAny<ClientRequest>()));
		}

		[TestMethod]
		public async Task WhenStateIsSetToGame_ThenRequestsAreRoutedToGameRouter()
		{
			var request = new ClientRequest { RequestName = "arbitrary_command" };
			var clientId = "arbitrary_id";
			_identityRepository.Setup(i => i.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new Identity { GameState = GameState.InGame });

			await _stateRouter.RouteAsync(clientId, request);

			_gameRouter.Verify(i => i.RouteAsync(It.IsAny<string>(), It.IsAny<ClientRequest>()));
		}
	}
}