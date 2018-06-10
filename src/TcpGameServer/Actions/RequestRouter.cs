using System.Collections.Generic;
using TcpGameServer.Contracts;
using TcpGameServer.Logging;

namespace TcpGameServer.Actions
{
	public interface IRequestRouter
	{
		void Route(string clientId, ClientRequest clientRequest);
	}

	public class RequestRouter : IRequestRouter
	{
		private readonly IReadOnlyDictionary<string, IRequestInserter> _inserterMap;
		private readonly ILogger _logger;

		public RequestRouter(ILogger logger, IDictionary<string, IRequestInserter> inserterMap)
		{
			_inserterMap = new Dictionary<string, IRequestInserter>(inserterMap);
			_logger = logger;
		}

		public void Route(string clientId, ClientRequest clientRequest)
		{
			if (_inserterMap.ContainsKey(clientRequest.RequestName))
			{
				_inserterMap[clientRequest.RequestName].Resolve(clientId, clientRequest);
				return;
			}

			_logger.Warning($"Route not found. ClientId {clientId}, Request: {clientRequest.RequestName}");
		}
	}
}