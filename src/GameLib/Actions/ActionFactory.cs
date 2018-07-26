using System;
using System.Collections.Generic;

namespace GameLib.Actions
{
  public abstract class ActionFactory
  {
    public abstract string ActionName { get; }
    public abstract Action Create(string ownerId, IDictionary<string, string> parameters);

    protected string GetRequired(string key, IDictionary<string, string> parameters)
    {
      if (parameters.ContainsKey(key))
      {
        var value = parameters[key];
        if (!string.IsNullOrEmpty(value))
        {
          return value;
        }
        else
        {
          throw new ArgumentException($"Required key '{key}' had empty value {GetKeyValues(parameters)}");
        }
      }
      throw new ArgumentException($"Required key '{key} was not found as parameter. {GetKeyValues(parameters)}");
    }

    private string GetKeyValues(IDictionary<string, string> parameters)
    {
      var result = Environment.NewLine + "Parameter kvp's:" + Environment.NewLine;
      foreach (var kvp in parameters)
      {
        result += $"{kvp.Key} - {kvp.Value}{Environment.NewLine}";
      }
      return result;
    }
  }
}