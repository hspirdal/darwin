using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TcpGameServer.Actions.Movement;
using System.Collections.Concurrent;
using System.Linq;
using System.Diagnostics;

namespace TcpGameServer.Actions
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
				_actionMap.Remove(action.OwnerId.ToString(), out Action a);
			}

			return actions;
		}

		public void PushInto(Action action)
		{
			var ownerId = action.OwnerId.ToString();
			var actionAlreadyStored = _actionMap.ContainsKey(ownerId);
			if (actionAlreadyStored == false)
			{
				var ableToAdd = _actionMap.TryAdd(ownerId, action);
				if (ableToAdd == false)
				{
					throw new InvalidOperationException($"Was not able to add action with id {ownerId}");
				}
			}
		}

		public int Count()
		{
			return _actionMap.Count();
		}
	}
}