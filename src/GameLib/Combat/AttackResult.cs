namespace GameLib.Combat
{
	public class AttackResult
	{
		public string AttackerId { get; set; }
		public string DefenderId { get; set; }
		public string AttackerName { get; set; } // tmp
		public string DefenderName { get; set; } // tmp
		public bool SuccessfulHit { get; set; }

		public int ToHitRoll { get; set; }
		public int ToHitModifier { get; set; }
		public int ToHitTotal => ToHitRoll + ToHitModifier;

		public int DamageRoll { get; set; }
		public int DamageModifier { get; set; }
		public int DamageTotal => DamageRoll + DamageModifier;

		public AttackResult(string attackerId, string defenderId, int toHitModifier, int damageModifier)
		{
			AttackerId = attackerId;
			DefenderId = defenderId;
			ToHitModifier = toHitModifier;
			DamageModifier = damageModifier;
		}
	}
}