using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using GameLib.Actions.Movement;
using System.Collections.Concurrent;
using System.Linq;
using System.Diagnostics;

namespace GameLib.Actions
{
	public interface IActionRepository
	{
		List<Action> PullOut();
		void PushInto(Action action);
		int Count();
	}

	public class ActionRepository : IActionRepository
	{
		private readonly ConcurrentDictionary<string, Action> _actionMap;

		public ActionRepository()
		{
			_actionMap = new ConcurrentDictionary<string, Action>();
		}

		public List<Action> PullOut()
		{
			var actions = _actionMap.Values.ToList();
			foreach (var action in actions)
			{
				_actionMap.TryRemove(action.OwnerId, out Action a);
			}

			return actions;
		}

		public void PushInto(Action action)
		{
			var actionAlreadyStored = _actionMap.ContainsKey(action.OwnerId);
			if (actionAlreadyStored == false)
			{
				var ableToAdd = _actionMap.TryAdd(action.OwnerId, action);
				if (ableToAdd == false)
				{
					throw new InvalidOperationException($"Was not able to add action with id {action.OwnerId}");
				}
			}
		}

		public int Count()
		{
			return _actionMap.Count();
		}
	}
}