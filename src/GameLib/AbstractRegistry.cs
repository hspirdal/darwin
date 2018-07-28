using System;
using System.Collections.Generic;
using System.Linq;

namespace GameLib
{
	public abstract class AbstractRegistry<T> where T : IIdentifiable
	{
		private readonly Dictionary<string, T> _map;

		public AbstractRegistry()
		{
			_map = new Dictionary<string, T>();
		}

		public bool Contains(string id)
		{
			return _map.ContainsKey(id);
		}

		public T Get(string id)
		{
			return _map[id];
		}

		public List<T> GetAll()
		{
			return _map.Values.ToList();
		}

		public void Register(T item)
		{
			if (_map.ContainsKey(item.Id))
			{
				throw new ArgumentException($"Already contains item of id '{item.Id}'");
			}

			_map.Add(item.Id, item);
		}

		public void Remove(string id)
		{
			_map.Remove(id);
		}
	}
}