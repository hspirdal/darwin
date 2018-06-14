using System.Threading.Tasks;

namespace TcpGameServer.Actions
{
    public interface IResolver
    {
        Task ResolveAsync(Action action);
    }
}