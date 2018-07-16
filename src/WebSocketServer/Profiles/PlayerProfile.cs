using AutoMapper;

namespace WebSocketServer.Profiles
{
	public class PlayerProfile : Profile
	{
		public PlayerProfile()
		{
			CreateMap<GameLib.Entities.Player, Client.Contracts.Entities.Player>();
		}
	}
}