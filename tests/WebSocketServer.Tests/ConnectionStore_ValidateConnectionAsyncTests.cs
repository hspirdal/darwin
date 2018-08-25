using Microsoft.VisualStudio.TestTools.UnitTesting;
using Autofac.Extras.Moq;
using Moq;
//using Microsoft.AspNetCore.SignalR;
using WebSocketServer;
using System;
using GameLib.Identities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace WebsocketServer.Tests
{
  [TestClass]
  public class ConnectionStore_ValidateConnectionAsyncTests
  {
    private AutoMock _container;
    private ConnectionStore _store;
    private Mock<IClientProxy> _clientProxy;

    [TestInitialize]
    public void GivenAConnectionStore()
    {
      _container = AutoMock.GetLoose();
      _store = _container.Create<ConnectionStore>();
      _clientProxy = new Mock<IClientProxy>();
    }

    [TestMethod]
    public async Task WhenSessionIdDoesNotMatch_ThenValidationReturnsFalse()
    {
      var connectionId = Guid.NewGuid().ToString();
      var invalidSessionId = Guid.NewGuid();
      var someStoredSessionId = Guid.NewGuid();

      var repository = _container.Mock<IIdentityRepository>();
      repository.Setup(i => i.GetAllAsync()).ReturnsAsync(new List<Identity> { new Identity { SessionId = someStoredSessionId } });

      var success = await _store.ValidateConnectionAsync(connectionId, invalidSessionId, _clientProxy.Object);

      Assert.IsFalse(success);
    }

    [TestMethod]
    public async Task WhenSessionIsValidButConnectionIdIsNotKnown_ThenConnectionIdGetsAddedToConnectionIdMap()
    {
      var connectionId = Guid.NewGuid().ToString();
      var sessionId = Guid.NewGuid();

      _store.RegisterNewConnection(new Connection { ConnectionId = "arbitrary", SessionId = sessionId, Client = _clientProxy.Object });

      var nullConnection = _store.GetById(connectionId);
      var success = await _store.ValidateConnectionAsync(connectionId, sessionId, _clientProxy.Object);
      var validConnection = _store.GetById(connectionId);

      Assert.IsNull(nullConnection);
      Assert.IsNotNull(validConnection);
    }

    [TestMethod]
    public async Task WhenSessionIsValidButConnectionIdIsNotKnown_ThenClientProxyIsUpdatedInBothSessionAndConnectionMap()
    {
      var connectionId = Guid.NewGuid().ToString();
      var sessionId = Guid.NewGuid();
      var newProxyClient = _container.Mock<IClientProxy>();

      _store.RegisterNewConnection(new Connection { ConnectionId = "arbitrary", SessionId = sessionId, Client = _clientProxy.Object });

      var oldClient = _store.GetById(sessionId).Client;
      var success = await _store.ValidateConnectionAsync(connectionId, sessionId, newProxyClient.Object);
      var connectionFromSessionId = _store.GetById(sessionId);
      var connectionFromConnectionId = _store.GetById(connectionId);

      Assert.AreSame(_clientProxy.Object, oldClient);
      Assert.AreSame(newProxyClient.Object, connectionFromConnectionId.Client);
      Assert.AreSame(newProxyClient.Object, connectionFromSessionId.Client);
    }

    [TestMethod]
    public async Task WhenConnectionIdIsFound_ThenValidationReturnsTrue()
    {
      var connectionId = Guid.NewGuid().ToString();
      var sessionId = Guid.NewGuid();

      var connection = new Connection { ConnectionId = connectionId };
      _store.RegisterNewConnection(connection);
      var connectionAddedCorrectly = _store.GetById(connectionId);

      var success = await _store.ValidateConnectionAsync(connectionId, sessionId, _clientProxy.Object);

      Assert.IsNotNull(connectionAddedCorrectly);
      Assert.IsTrue(success);
    }

    [TestMethod]
    public async Task WhenSessionIdIsFound_ThenValidationReturnsTrue()
    {
      var connectionId = Guid.NewGuid().ToString();
      var sessionId = Guid.NewGuid();

      var connection = new Connection { ConnectionId = "arbitraryId", SessionId = sessionId };
      _store.RegisterNewConnection(connection);
      var connectionAddedCorrectly = _store.GetById(sessionId);

      var success = await _store.ValidateConnectionAsync(connectionId, sessionId, _clientProxy.Object);

      Assert.IsNotNull(connectionAddedCorrectly);
      Assert.IsTrue(success);
    }

    [TestMethod]
    public async Task WhenSessionIsStoredInRepository_ThenValidationReturnsTrue()
    {
      var connectionId = Guid.NewGuid().ToString();
      var sessionId = Guid.NewGuid();

      var repository = _container.Mock<IIdentityRepository>();
      repository.Setup(i => i.GetAllAsync()).ReturnsAsync(new List<Identity> { new Identity { SessionId = sessionId } });

      var success = await _store.ValidateConnectionAsync(connectionId, sessionId, _clientProxy.Object);
      Assert.IsTrue(success);
    }

    [TestMethod]
    public async Task WhenValidatingConnectionUsingEmptyConnectionId_ThenValidationReturnsFalse()
    {
      var connectionId = string.Empty;
      var arbitrarySessionId = Guid.NewGuid(); ;

      var success = await _store.ValidateConnectionAsync(connectionId, arbitrarySessionId, _clientProxy.Object);
      Assert.IsFalse(success);
    }

    [TestMethod]
    public async Task WhenValidatingConnectionUsingEmptySessionId_ThenValidationReturnsFalse()
    {
      var connectionId = "arbitrary";
      var sessionId = Guid.Empty;

      var success = await _store.ValidateConnectionAsync(connectionId, sessionId, _clientProxy.Object);
      Assert.IsFalse(success);
    }
  }
}
