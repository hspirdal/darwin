using TcpGameServer.Contracts;

namespace TcpGameServer.Actions
{
	public interface IRequestInserter
	{
		void Resolve(string clientId, ClientRequest clientRequest);
	}
}