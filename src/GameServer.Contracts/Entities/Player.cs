using TcpGameServer.Contracts.Properties;

namespace TcpGameServer.Contracts.Entities
{
	public class Player
	{
		public string Name { get; set; }
		public Inventory Inventory { get; set; }
	}
}