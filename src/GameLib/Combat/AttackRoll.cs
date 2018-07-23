using GameLib.Entities;

namespace GameLib.Combat
{
	public class AttackRoll
	{
		public AttackRoll() { Name = string.Empty; }

		public AttackRoll(Weapon weapon, int toHitModifier, int damageModifier)
		{
			Name = weapon.Name;
			TimesApplied = weapon.TimesApplied;
			DiceType = weapon.DiceType;
			ToHitModifier = toHitModifier;
			DamageModifier = damageModifier;
		}

		public string Name { get; set; }
		public int TimesApplied { get; set; }
		public int DamageModifier { get; set; }
		public int ToHitModifier { get; set; }
		public DiceType DiceType { get; set; }

		public override string ToString()
		{
			return $"{Name} +{ToHitModifier} ({TimesApplied}{DiceType}+{DamageModifier})";
		}
	}
}