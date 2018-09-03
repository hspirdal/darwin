using System;
using System.Threading.Tasks;
using Client.Contracts;
using GameLib.Properties;
using GameLib.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace WebSocketServer.Tests
{
	[TestClass]
	public class QueryResolverTests
	{
		private string _validUserId;
		private string _validQuery;
		private QueryResolver _resolver;

		[TestInitialize]
		public void Arrange()
		{
			_validUserId = "someId";
			_validQuery = "Query.GetUserState";

			var userRepository = new Mock<IUserRepository>();
			_resolver = new QueryResolver(userRepository.Object);

			userRepository.Setup(i => i.GetByIdOrDefaultAsync(It.Is<string>(x => x == _validUserId))).ReturnsAsync(new User { GameState = GameState.InGame });
			userRepository.Setup(i => i.GetByIdOrDefaultAsync(It.Is<string>(x => x != _validUserId))).ReturnsAsync(default(User));
		}

		[TestMethod]
		public async Task WhenQueryingForGameStateWithValidParameters_ThenResolverReturnsResponseWithGameState()
		{
			var request = new ClientRequest { UserId = _validUserId, RequestName = _validQuery };

			var response = await _resolver.ResolveAsync(request);
			var gameState = JsonConvert.DeserializeObject<string>(response.Payload);

			Assert.AreEqual(GameState.InGame.ToString(), gameState);
		}

		[TestMethod]
		public async Task WhenQueryingForGameStateWithUnknownUserId_ThenArgumentExceptionIsThrown()
		{
			var request = new ClientRequest { UserId = "invalid_userid", RequestName = _validQuery };

			await Assert.ThrowsExceptionAsync<ArgumentException>(() => _resolver.ResolveAsync(request));
		}

		[TestMethod]
		public async Task WhenQueryingForGameStateWithUnknownQuery_ThenArgumentExceptionIsThrown()
		{
			var request = new ClientRequest { UserId = _validUserId, RequestName = "invalid_query" };

			await Assert.ThrowsExceptionAsync<ArgumentException>(() => _resolver.ResolveAsync(request));
		}
	}
}