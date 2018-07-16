using AutoMapper;

namespace WebSocketServer.Profiles
{
	public class PlayerProfile : Profile
	{
		public PlayerProfile()
		{
			CreateMap<GameLib.Entities.Player, TcpGameServer.Contracts.Entities.Player>();
		}
	}
}