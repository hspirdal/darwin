using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GameLib.Actions;
using Client.Contracts;
using System.Threading.Tasks;
using GameLib.Entities;
using GameLib.Properties;
using GameLib.Users;

namespace GameLib.Tests.Actions
{
	[TestClass]
	public class StateRouterTests
	{
		private Mock<IUserRepository> _userRepository;
		private Mock<ILobbyRouter> _lobbyRouter;
		private Mock<IGameRouter> _gameRouter;
		private StateRequestRouter _stateRouter;

		[TestInitialize]
		public void GivenStateWithGameAndLobbyRouter()
		{
			var container = AutoMock.GetLoose();
			_lobbyRouter = container.Mock<ILobbyRouter>();
			_gameRouter = container.Mock<IGameRouter>();
			_userRepository = container.Mock<IUserRepository>();
			_stateRouter = container.Create<StateRequestRouter>();
		}

		[TestMethod]
		public async Task WhenStateIsSetToLobby_ThenRequestsAreRoutedToLobbyRouter()
		{
			var request = new ClientRequest { RequestName = "arbitrary_command" };
			var userId = "arbitrary_id";
			_userRepository.Setup(i => i.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new User { Id = userId, GameState = GameState.GameLobby });

			await _stateRouter.RouteAsync(userId, request);

			_lobbyRouter.Verify(i => i.RouteAsync(It.IsAny<string>(), It.IsAny<ClientRequest>()));
		}

		[TestMethod]
		public async Task WhenStateIsSetToGame_ThenRequestsAreRoutedToGameRouter()
		{
			var request = new ClientRequest { RequestName = "arbitrary_command" };
			var userId = "arbitrary_id";
			_userRepository.Setup(i => i.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(new User { Id = userId, GameState = GameState.InGame });

			await _stateRouter.RouteAsync(userId, request);

			_gameRouter.Verify(i => i.RouteAsync(It.IsAny<string>(), It.IsAny<ClientRequest>()));
		}
	}
}