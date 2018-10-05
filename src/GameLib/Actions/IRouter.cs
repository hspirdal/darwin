using System.Threading.Tasks;
using Client.Contracts;

namespace GameLib.Actions
{
	public interface IRouter
	{
		Task<ServerResponse> RouteAsync(string userId, ClientRequest clientRequest);
	}
}