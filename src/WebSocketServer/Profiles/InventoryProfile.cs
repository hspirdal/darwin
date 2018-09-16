using System.Linq;
using AutoMapper;
using Client.Contracts.Entities;

namespace WebSocketServer.Profiles
{
	public class InventoryProfile : Profile
	{
		public InventoryProfile()
		{
			CreateMap<GameLib.Properties.Inventory, Client.Contracts.Properties.Inventory>()
			.ForMember(a => a.Items, m => m.MapFrom(s => s.Items.Select(i => new Entity { Id = i.Id, Name = i.Name, Type = i.Type })));
		}
	}
}