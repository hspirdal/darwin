using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLib.Properties.Autonomy
{
	public interface IAutonomousRegistry
	{
		bool Contains(string id);
		IAutonomous Get(string id);
		List<IAutonomous> GetAll();
		void Register(IAutonomous autonomous);
		void Remove(string id);
	}

	public class AutonomousRegistry : IAutonomousRegistry
	{
		private readonly Dictionary<string, IAutonomous> _autonomousMap;

		public AutonomousRegistry()
		{
			_autonomousMap = new Dictionary<string, IAutonomous>();
		}

		public bool Contains(string id)
		{
			return _autonomousMap.ContainsKey(id);
		}

		public IAutonomous Get(string id)
		{
			return _autonomousMap[id];
		}

		public void Register(IAutonomous autonomous)
		{
			if (_autonomousMap.ContainsKey(autonomous.Id) == false)
			{
				_autonomousMap.Add(autonomous.Id, autonomous);
				return;
			}

			throw new ArgumentException($"Autonomous entity with '{autonomous.Id}' already exists in registry");
		}

		public List<IAutonomous> GetAll()
		{
			return _autonomousMap.Values.ToList();
		}

		public void Remove(string id)
		{
			if (_autonomousMap.ContainsKey(id))
			{
				_autonomousMap.Remove(id);
				return;
			}

			throw new ArgumentException($"Could not remove autonomous entity with id '{id}' as it was not found in registry");
		}
	}
}