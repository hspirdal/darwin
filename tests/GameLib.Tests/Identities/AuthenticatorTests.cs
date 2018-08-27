using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLib.Users;
using Moq;

namespace GameLib.Tests.Identities
{
	[TestClass]
	public class AuthenticatorTests
	{
		private Authenticator _authenticator;
		private string _validUserId;
		private Guid _validSessionId;

		[TestInitialize]
		public void GivenIdentityRepositoryWithOneIdentity()
		{
			var container = AutoMock.GetLoose();
			var userRepository = container.Mock<IUserRepository>();
			_validUserId = "someId";
			_validSessionId = Guid.NewGuid();

			var validUser = new User { Id = _validUserId, SessionId = _validSessionId };
			userRepository.Setup(i => i.GetByIdOrDefaultAsync(It.Is<string>(id => id == _validUserId))).ReturnsAsync(validUser);
			userRepository.Setup(i => i.GetByIdOrDefaultAsync(It.Is<string>(id => id != _validUserId))).ReturnsAsync(default(User));

			_authenticator = container.Create<Authenticator>();
		}

		[TestMethod]
		public async Task WhenUserIdAndSessionIdMatch_ThenAuthenticationReturnsTrue()
		{
			var isAuthenticated = await _authenticator.AuthenticateAsync(_validUserId, _validSessionId);

			Assert.IsTrue(isAuthenticated);
		}

		[TestMethod]
		public async Task WhenUserIdIsInvalid_ThenAuthenticationReturnsFalse()
		{
			var invalidUserId = "invalidId";
			var isAuthenticated = await _authenticator.AuthenticateAsync(invalidUserId, _validSessionId);

			Assert.IsFalse(isAuthenticated);
		}

		[TestMethod]
		public async Task WhenSessionIdIsInvalid_ThenAuthenticationReturnsFalse()
		{
			var invalidSessionId = Guid.NewGuid();
			var isAuthenticated = await _authenticator.AuthenticateAsync(_validUserId, invalidSessionId);

			Assert.IsFalse(isAuthenticated);
		}

		[TestMethod]
		public async Task WhenUserIdAndSessionIdIsInvalid_ThenAuthenticationReturnsFalse()
		{
			var invalidUserId = "invalidId";
			var invalidSessionId = Guid.NewGuid();
			var isAuthenticated = await _authenticator.AuthenticateAsync(invalidUserId, invalidSessionId);

			Assert.IsFalse(isAuthenticated);
		}
	}
}