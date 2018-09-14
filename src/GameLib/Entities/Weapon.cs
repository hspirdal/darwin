using GameLib.Combat;
using GameLib.Dice;
using GameLib.Utility;

namespace GameLib.Entities
{
	public class Weapon : Item, IDeepCopy<Weapon>
	{
		public AttackType AttackType { get; set; }
		public DamageType DamageType { get; set; }
		public DiceType DiceType { get; set; }
		public int TimesApplied { get; set; }
		public int CriticalRange { get; set; }
		public int CriticalModifier { get; set; }

		public Weapon()
			: base(id: string.Empty, name: string.Empty, SubType.Weapon)
		{
			TimesApplied = 1;
			CriticalRange = 20;
			CriticalModifier = 2;
		}
		public Weapon(string id, string name, DamageType damageType, DiceType diceType, int timesApplied, int criticalRange, int criticalModifier)
			: base(id, name, SubType.Weapon)
		{
			DamageType = damageType;
			DiceType = diceType;
			TimesApplied = timesApplied;
			CriticalRange = criticalRange;
			CriticalModifier = criticalModifier;
		}

		public new Weapon DeepCopy()
		{
			return new Weapon(string.Copy(Id), string.Copy(Name), DamageType, DiceType, TimesApplied, CriticalRange, CriticalModifier);
		}
	}
}