using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameLib.Identities;

namespace GameLib.Tests.Identities
{
	[TestClass]
	public class AuthenticatorTests
	{
		private Authenticator _authenticator;

		[TestInitialize]
		public void GivenIdentityRepositoryWithOneIdentity()
		{
			var container = AutoMock.GetLoose();

			var identityRepository = container.Mock<IIdentityRepository>();
			identityRepository.Setup(i => i.GetAll()).Returns(new List<Identity>()
			{
				new Identity
				{
					Id = "1",
					UserName = "jools",
					Password = "1234"
				}
			});

			_authenticator = container.Create<Authenticator>();
		}

		[TestMethod]
		public void WhenAuthenticatingWithParametersThatMatch_ThenAuthentificationReturnsValidIdentity()
		{
			var request = new AuthentificationRequest
			{
				UserName = "jools",
				Password = "1234",
				ConnectionId = Guid.NewGuid()
			};

			var identity = _authenticator.Authenticate(request);

			Assert.IsNotNull(identity);
		}

		[TestMethod]
		public void WhenAuthenticatingWithParametersThatDoesNotMatch_ThenAuthentificationReturnsNull()
		{
			var request = new AuthentificationRequest();
			var identity = _authenticator.Authenticate(request);

			Assert.IsNull(identity);
		}
	}
}