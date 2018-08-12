using System;
using System.Collections.Concurrent;

namespace GameLib
{
	public abstract class AbstractConcurrentRegistry<T>
	{
		private readonly ConcurrentDictionary<string, T> _map;

		public AbstractConcurrentRegistry()
		{
			_map = new ConcurrentDictionary<string, T>();
		}

		public bool Contains(string id)
		{
			return _map.ContainsKey(id);
		}

		public T Get(string id)
		{
			if (_map.TryGetValue(id, out var value))
			{
				return value;
			}

			return default(T);
		}

		public void Register(string id, T obj)
		{
			_map.AddOrUpdate(id, obj, (string key, T o) =>
			{
				return o;
			});
		}

		public void Remove(string id)
		{
			if (_map.TryRemove(id, out var value))
			{
				return;
			}

			// Handle in the future if logs are showing that it triggers.
			throw new InvalidOperationException($"Could not remove id '{id}' from registry");
		}

	}
}