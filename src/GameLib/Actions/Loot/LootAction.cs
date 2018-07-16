namespace GameLib.Actions.Loot
{
	public class LootAction : Action
	{
		public string ItemId { get; set; }

		public override bool IsValid()
		{
			return base.IsValid() && !string.IsNullOrEmpty(ItemId);
		}

		public override string ToString()
		{
			return base.ToString() + $", {nameof(ItemId)}: '{ItemId}'";
		}
	}
}