using System.Threading.Tasks;
using TcpGameServer.Contracts;

namespace GameLib.Actions
{
    public interface IRouter
    {
        Task RouteAsync(string clientId, ClientRequest clientRequest);
    }
}