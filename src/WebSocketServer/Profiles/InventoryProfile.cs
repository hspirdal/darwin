using System.Linq;
using AutoMapper;

namespace WebSocketServer.Profiles
{
	public class InventoryProfile : Profile
	{
		public InventoryProfile()
		{
			CreateMap<GameLib.Players.Inventory, TcpGameServer.Contracts.Players.Inventory>()
			.ForMember(a => a.Items, m => m.MapFrom(s => s.Items.Select(i => i.Name)));
		}
	}
}