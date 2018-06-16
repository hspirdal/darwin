using TcpGameServer.Contracts;

namespace GameLib.Actions
{
    public interface IRouter
    {
        void Route(string clientId, ClientRequest clientRequest);
    }
}