using System;
using System.Collections.Generic;

namespace Darwin.Api.Actions
{
	public interface IActionRepository
	{
		void Add(IAction action);
		bool AbleToAdd(int ownerIId);
		DateTime NextActionAvailable { get; }
		void ClearAll();
	}

	public class ActionRepository : IActionRepository
	{
		private readonly Dictionary<int, IAction> _actionMap;
		private DateTime _lastCleared;

		public ActionRepository()
		{
			_actionMap = new Dictionary<int, IAction>();
			_lastCleared = DateTime.UtcNow;
		}

		public DateTime NextActionAvailable => _lastCleared.AddSeconds(1);

		public void Add(IAction action)
		{
			if (_actionMap.ContainsKey(action.OwnerId) == false)
			{
				_actionMap.Add(action.OwnerId, action);
				return;
			}

			throw new ArgumentException($"Action already queued for this timeslot. [ownerId: {action.OwnerId}, action: {action.Name}");
		}

		public bool AbleToAdd(int ownerIId)
		{
			return !_actionMap.ContainsKey(ownerIId);
		}

		public void ClearAll()
		{
			_actionMap.Clear();
			_lastCleared = DateTime.UtcNow;
		}
	}
}