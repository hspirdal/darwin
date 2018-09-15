using System.Collections.Generic;

namespace GameLib.Actions.Consume
{
	public class ConsumeActionFactory : ActionFactory
	{
		public override string ActionName => ConsumeAction.CanonicalName;

		public override Action Create(string ownerId, IDictionary<string, string> parameters)
		{
			var itemId = GetRequired("ItemId", parameters);
			return new ConsumeAction(ownerId, itemId);
		}
	}
}