using System.Linq;
using AutoMapper;

namespace WebSocketServer.Profiles
{
	public class CellProfile : Profile
	{
		public CellProfile()
		{
			CreateMap<GameLib.Area.Cell, Client.Contracts.Area.Cell>()
			.ForMember(i => i.Creatures, opt => opt.MapFrom(x => x.Content.Entities.Where(i => i.Type == "Player")))
			.ForMember(i => i.Items, opt => opt.MapFrom(x => x.Content.Entities.Where(i => i.Type == "Item")));
		}
	}
}