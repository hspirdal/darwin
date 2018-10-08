using System.Collections.Generic;
using Client.Contracts;
using GameLib.Logging;
using System.Threading.Tasks;
using System;

namespace GameLib.Actions
{
	public interface IGameRouter : IRouter { }

	public class GameRouter : IGameRouter
	{
		private readonly ILogger _logger;
		private readonly IActionInserter _actionInserter;

		public GameRouter(ILogger logger, IActionInserter actionInserter)
		{
			_logger = logger;
			_actionInserter = actionInserter;
		}

		public Task<ServerResponse> RouteAsync(string userId, ClientRequest clientRequest)
		{
			if (string.IsNullOrEmpty(clientRequest?.RequestName))
			{
				throw new ArgumentException($"Route was empty. ClientId {userId}");
			}

			if (!clientRequest.RequestName.Contains("Action."))
			{
				_logger.Warn($"Request name '{clientRequest.RequestName}' does not follow correct naming and will be ignored");
				return Task.FromResult(new ServerResponse(ResponseType.RequestMalformed, "Request expected to prefix '.Action'"));
			}

			try
			{
				_actionInserter.Insert(userId, clientRequest.RequestName, clientRequest.Payload);
				return Task.FromResult(new ServerResponse(ResponseType.RequestAccepted, clientRequest.RequestName));
			}
			catch (Exception e)
			{
				_logger.Error(e);
			}

			return Task.FromResult(new ServerResponse(ResponseType.RequestDeclined, "Action not processed"));
		}
	}
}