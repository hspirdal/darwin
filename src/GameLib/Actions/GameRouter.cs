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

    public Task RouteAsync(string clientId, ClientRequest clientRequest)
    {
      if (string.IsNullOrEmpty(clientRequest?.RequestName))
      {
        _logger.Warn($"Route was empty. ClientId {clientId}");
        return Task.CompletedTask;
      }

      if (!clientRequest.RequestName.Contains("Action."))
      {
        _logger.Warn($"Request name '{clientRequest.RequestName}' does not follow correct naming and will be ignored");
        return Task.CompletedTask;
      }

      try
      {
        // TODO: handle response.
        var response = _actionInserter.Insert(clientId, clientRequest.RequestName, clientRequest.Payload);
      }
      catch (Exception e)
      {
        _logger.Error(e);
      }

      return Task.CompletedTask;
    }
  }
}