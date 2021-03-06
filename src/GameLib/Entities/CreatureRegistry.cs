using System;
using System.Collections.Generic;
using System.Linq;
using GameLib.Logging;

namespace GameLib.Entities
{
	public interface ICreatureRegistry
	{
		Creature GetById(string id);
		List<Creature> GetAll();
		void Register(Creature creature);
		void Remove(string id);
	}

	public class CreatureRegistry : ICreatureRegistry
	{
		private readonly Dictionary<string, Creature> _creatureMap;

		public CreatureRegistry()
		{

			_creatureMap = new Dictionary<string, Creature>();
		}

		public List<Creature> GetAll()
		{
			return _creatureMap.Values.ToList();
		}

		public Creature GetById(string id)
		{
			if (_creatureMap.ContainsKey(id))
			{
				return _creatureMap[id];
			}

			throw new ArgumentException($"Could not find creature with id '{id}' in registry");
		}

		public void Register(Creature creature)
		{
			Validate(creature);

			_creatureMap.Add(creature.Id, creature);
		}

		public void Remove(string id)
		{
			if (_creatureMap.ContainsKey(id))
			{
				_creatureMap.Remove(id);
				return;
			}

			throw new ArgumentException($"Could not remove creature with id '{id}' from registry as it was not found");
		}

		private void Validate(Creature creature)
		{
			if (creature == null || string.IsNullOrEmpty(creature.Id))
			{
				throw new ArgumentException($"Could not register creature because it was eiter null or id was empty");
			}
			if (_creatureMap.ContainsKey(creature.Id))
			{
				throw new ArgumentException($"Creature with id '{creature.Id}' already exists in registry");
			}
		}
	}
}