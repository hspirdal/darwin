using TcpGameServer.Contracts;

namespace GameLib.Actions
{
	public interface IRequestInserter
	{
		void Resolve(string clientId, ClientRequest clientRequest);
	}
}