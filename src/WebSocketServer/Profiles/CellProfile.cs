using AutoMapper;

namespace WebSocketServer.Profiles
{
	public class CellProfile : Profile
	{
		public CellProfile()
		{
			CreateMap<GameLib.Area.Cell, TcpGameServer.Contracts.Area.Cell>()
			.ForMember(i => i.Content, opt => opt.MapFrom(x => x.Content.Entities));
		}
	}
}