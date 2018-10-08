using System;
using System.Collections.Concurrent;
using GameLib.Utility;

namespace GameLib.Actions
{
	public interface ICooldownRegistry
	{
		bool IsCooldownInEffect(string userId);
		DateTime TimeUntilCooldownIsDone(string userId);
		bool Add(Cooldown cooldown);
	}

	public class CooldownRegistry : ICooldownRegistry
	{
		private readonly IClock _clock;
		private readonly ConcurrentDictionary<string, Cooldown> _cooldownMap;

		public CooldownRegistry(IClock clock)
		{
			_cooldownMap = new ConcurrentDictionary<string, Cooldown>();
			_clock = clock;
		}

		public int Count => _cooldownMap.Count;

		public bool Add(Cooldown cooldown)
		{
			if (string.IsNullOrEmpty(cooldown?.UserId))
			{
				throw new ArgumentException("Invalid cooldown parameters.");
			}

			if (IsCooldownInEffect(cooldown.UserId))
			{
				return false;
			}

			_cooldownMap.AddOrUpdate(cooldown.UserId, cooldown, (string key, Cooldown c) =>
			{
				return cooldown;
			});

			return true;
		}

		public bool IsCooldownInEffect(string userId)
		{
			if (_cooldownMap.ContainsKey(userId))
			{
				if (_cooldownMap.TryGetValue(userId, out Cooldown cooldown))
				{
					return _clock.UtcNow < cooldown.ValidTo;
				}

				throw new InvalidOperationException($"Failed to retrieve cooldown for user with id '{userId}'");
			}

			return false;
		}

		public DateTime TimeUntilCooldownIsDone(string userId)
		{
			if (_cooldownMap.ContainsKey(userId) && _cooldownMap.TryGetValue(userId, out Cooldown cooldown))
			{
				return cooldown.ValidTo;
			}
			return DateTime.UtcNow;
		}
	}
}