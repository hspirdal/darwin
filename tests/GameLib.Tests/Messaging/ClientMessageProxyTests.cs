using Client.Contracts;
using GameLib.Messaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameLib.Tests.Messaging
{
	[TestClass]
	public class ClientMessageProxyTests
	{
		[TestMethod]
		public void WhenClientMessageProxyReceivesMessage_ThenClientSenderPassesMessageOn()
		{
			var clientSender = new Mock<IClientSender>();
			var proxy = new ClientMessageProxy(clientSender.Object, "arbitraryId");
			var gameMessage = new GameMessage();

			proxy.Receive(gameMessage);

			clientSender.Verify(i => i.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ServerResponse>()), Times.Once);
		}
	}
}