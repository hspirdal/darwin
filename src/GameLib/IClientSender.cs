using System.Threading.Tasks;
using Client.Contracts;

namespace GameLib
{
	public interface IClientSender
	{
		Task SendAsync(string userId, string channel, ServerResponse serverResponse);
	}
}