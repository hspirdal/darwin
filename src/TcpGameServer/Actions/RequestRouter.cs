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
			if (string.IsNullOrEmpty(clientRequest?.RequestName))
			{
				_logger.Warn($"Route was empty. ClientId {clientId}");
				return;
			}

			var key = clientRequest.RequestName.ToLower();
			if (_inserterMap.ContainsKey(key))
			{
				_inserterMap[key].Resolve(clientId, clientRequest);
				return;
			}

			_logger.Warn($"Route not found. ClientId {clientId}, Request: {clientRequest.RequestName}");
		}
	}
}