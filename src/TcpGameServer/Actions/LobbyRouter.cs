using TcpGameServer.Contracts;

namespace TcpGameServer.Actions
{
    public interface ILobbyRouter : IRouter { }

    public class LobbyRouter : ILobbyRouter
    {
        public void Route(string clientId, ClientRequest clientRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}