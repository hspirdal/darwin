using System.Threading.Tasks;
using Client.Contracts;

namespace GameLib.Actions
{
	public interface IRouter
	{
		Task RouteAsync(string clientId, ClientRequest clientRequest);
	}
}