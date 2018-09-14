using GameLib.Actions.Consume;
using GameLib.Utility;

namespace GameLib.Entities
{
	public class Potion : Entity, IDeepCopy<Potion>
	{
		public EffectType EffectType { get; set; }
		public EffectTarget EffectTarget { get; set; }
		public int Amount { get; set; }

		public Potion()
			: base(id: string.Empty, name: string.Empty, type: nameof(Potion))
		{
		}

		public Potion(string id, string name, EffectType effectType, EffectTarget effectTarget, int amount)
			: base(id, name, nameof(Potion))
		{
			EffectType = effectType;
			EffectTarget = effectTarget;
			Amount = amount;
		}

		public new Potion DeepCopy()
		{
			return new Potion(string.Copy(Id), string.Copy(Name), EffectType, EffectTarget, Amount);
		}
	}
}