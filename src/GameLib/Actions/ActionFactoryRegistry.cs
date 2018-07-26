using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GameLib.Actions
{
  public interface IActionFactoryRegistry
  {
    Action Create(string ownerId, string actionName, string actionPayload);
  }

  public class ActionFactoryRegistry : IActionFactoryRegistry
  {
    private readonly IReadOnlyDictionary<string, ActionFactory> _actionFactoryMap;
    private readonly JsonSerializerSettings _serializerSettings;

    public ActionFactoryRegistry(IReadOnlyDictionary<string, ActionFactory> actionFactoryMap)
    {
      _actionFactoryMap = actionFactoryMap;
      _serializerSettings = new JsonSerializerSettings();
      _serializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
      _serializerSettings.NullValueHandling = NullValueHandling.Ignore;
    }

    public Action Create(string ownerId, string actionName, string actionPayload)
    {
      if (_actionFactoryMap.ContainsKey(actionName))
      {
        var parameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(actionPayload, _serializerSettings);
        return _actionFactoryMap[actionName].Create(ownerId, parameters);
      }

      throw new ArgumentException($"Could not find action factory with name '{actionName}' for ownerId '{ownerId}'");
    }
  }
}