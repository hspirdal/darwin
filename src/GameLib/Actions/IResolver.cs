using System.Threading.Tasks;

namespace GameLib.Actions
{
    public interface IResolver
    {
        Task ResolveAsync(Action action);
    }
}