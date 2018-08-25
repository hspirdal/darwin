using System;
using Autofac.Extras.Moq;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebSocketServer;

namespace WebsocketServer.Tests
{
  [TestClass]
  public class ConnectionStore_RemoveTests
  {
    private AutoMock _container;
    private ConnectionStore _store;
    private Mock<IClientProxy> _clientProxy;
    private string _connectionId;

    [TestInitialize]
    public void GivenAConnectionStoreWithOneConnection()
    {
      _container = AutoMock.GetLoose();
      _store = _container.Create<ConnectionStore>();
      _clientProxy = new Mock<IClientProxy>();
      _connectionId = Guid.NewGuid().ToString();
      var arbitrarySessionId = Guid.NewGuid();
      var connection = new Connection { ConnectionId = _connectionId, SessionId = arbitrarySessionId };

      _store.RegisterNewConnection(connection);
    }

    [TestMethod]
    public void WhenRemovingConnectionId_ThenItWillBeRemoved()
    {
      var connectionFound = _store.GetById(_connectionId);
      _store.Remove(_connectionId);
      var notFoundConnection = _store.GetById(_connectionId);

      Assert.IsNotNull(connectionFound);
      Assert.IsNull(notFoundConnection);
    }

    [TestMethod]
    public void WhenRemovingConnectionFromConnectionMap_ThenSessionMapIsUnaffected()
    {
      var connection = _store.GetById(_connectionId);
      _store.Remove(_connectionId);
      var storedConnection = _store.GetById(connection.SessionId);

      Assert.IsNotNull(storedConnection);
    }
  }
}