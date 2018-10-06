using System;
using System.Collections.Concurrent;

namespace GameLib
{
	public abstract class AbstractConcurrentRegistry<K, V>
	{
		private readonly ConcurrentDictionary<K, V> _map;

		protected AbstractConcurrentRegistry()
		{
			_map = new ConcurrentDictionary<K, V>();
		}

		public bool Contains(K id)
		{
			return _map.ContainsKey(id);
		}

		public V Get(K id)
		{
			if (_map.TryGetValue(id, out var value))
			{
				return value;
			}

			return default(V);
		}

		public void RegisterOrUpdate(K id, V obj)
		{
			_map.AddOrUpdate(id, obj, (K key, V o) => obj);
		}

		public void Remove(K id)
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