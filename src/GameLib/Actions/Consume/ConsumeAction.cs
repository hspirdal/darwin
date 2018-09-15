namespace GameLib.Actions.Consume
{
	public class ConsumeAction : Action
	{
		public static string CanonicalName => "Action.Consume";
		public string ItemId { get; set; }

		public ConsumeAction(string ownerId, string itemId)
			: base(ownerId, name: CanonicalName, cooldownDurationMilisecs: 100)
		{
			ItemId = itemId;
		}

		public override string ToString()
		{
			return base.ToString() + $", {nameof(ItemId)}: '{ItemId}'";
		}
	}
}