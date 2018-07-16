using System.Collections.Generic;
using Client.Contracts;
using GameLib.Logging;
using System.Threading.Tasks;

namespace GameLib.Actions
{
	public interface IGameRouter : IRouter { }

	public class GameRouter : IGameRouter
	{
		private readonly IReadOnlyDictionary<string, IRequestInserter> _inserterMap;
		private readonly ILogger _logger;

		public GameRouter(ILogger logger, IDictionary<string, IRequestInserter> inserterMap)
		{
			_inserterMap = new Dictionary<string, IRequestInserter>(inserterMap);
			_logger = logger;
		}

		public Task RouteAsync(string clientId, ClientRequest clientRequest)
		{
			if (string.IsNullOrEmpty(clientRequest?.RequestName))
			{
				_logger.Warn($"Route was empty. ClientId {clientId}");
				return Task.CompletedTask;
			}

			var key = clientRequest.RequestName.ToLower();
			if (_inserterMap.ContainsKey(key))
			{
				_inserterMap[key].Resolve(clientId, clientRequest);
				return Task.CompletedTask;
			}

			_logger.Warn($"Route not found. ClientId {clientId}, Request: {clientRequest.RequestName}");
			return Task.CompletedTask;
		}
	}
}