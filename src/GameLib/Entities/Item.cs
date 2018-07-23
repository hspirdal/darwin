using GameLib.Utility;

namespace GameLib.Entities
{
	public class Item : Entity, IDeepCopy<Item>
	{
		public Item()
			: base(id: string.Empty, name: string.Empty, type: nameof(Item))
		{
		}

		public Item(string id, string name, string type)
			: base(id, name, type)
		{
		}

		public new Item DeepCopy()
		{
			return new Item(string.Copy(Id), string.Copy(Name), string.Copy(Type));
		}
	}
}