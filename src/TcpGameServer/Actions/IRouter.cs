using TcpGameServer.Contracts;

namespace TcpGameServer.Actions
{
    public interface IRouter
    {
        void Route(string clientId, ClientRequest clientRequest);
    }
}