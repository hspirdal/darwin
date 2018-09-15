using System;
using GameLib.Actions.Consume;
using GameLib.Entities;

namespace GameLib.Entities
{
	public interface IPotionFactory
	{
		Potion CreateByName(string name);
	}
	public class PotionFactory : IPotionFactory
	{
		public Potion CreateByName(string name)
		{
			if (name == "Small Healing Potion")
			{
				return new Potion(Guid.NewGuid().ToString(), name, EffectType.Replenish, EffectTarget.HitPoints, 8);
			}
			else
			{
				throw new ArgumentException($"Unknown potion by name '{name}'");
			}
		}
	}
}