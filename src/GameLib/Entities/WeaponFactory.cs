using System;
using System.Collections.Generic;

namespace GameLib.Entities
{
	public interface IWeaponFactory
	{
		Weapon Create(string name);
	}
	public class WeaponFactory : IWeaponFactory
	{
		private readonly Dictionary<string, Weapon> _weaponMap;

		public WeaponFactory(List<Weapon> templates)
		{
			_weaponMap = new Dictionary<string, Weapon>();
			foreach (var weapon in templates)
			{
				_weaponMap.Add(weapon.Name, weapon);
			}
		}

		public Weapon Create(string name)
		{
			if (_weaponMap.ContainsKey(name))
			{
				var weapon = _weaponMap[name].DeepCopy();
				weapon.Id = Guid.NewGuid().ToString();
				return weapon;
			}

			throw new ArgumentException($"Could not find weapon with name '{name}'");
		}
	}
}