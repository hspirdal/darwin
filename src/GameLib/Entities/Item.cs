using GameLib.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GameLib.Entities
{
	public enum SubType { None, Weapon, Armor, Potion, Container }

	public class Item : Entity, IDeepCopy<Item>
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public SubType SubType { get; set; }

		public Item()
			: base(id: string.Empty, name: string.Empty, type: nameof(Item))
		{
			SubType = SubType.None;
		}

		public Item(string id, string name, SubType subType)
			: base(id, name, type: nameof(Item))
		{
			SubType = subType;
		}

		public new Item DeepCopy()
		{
			return new Item(string.Copy(Id), string.Copy(Name), SubType);
		}
	}
}