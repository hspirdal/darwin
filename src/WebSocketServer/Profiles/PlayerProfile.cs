using AutoMapper;

namespace WebSocketServer.Profiles
{
	public class PlayerProfile : Profile
	{
		public PlayerProfile()
		{
			CreateMap<GameLib.Players.Player, TcpGameServer.Contracts.Players.Player>();
		}
	}
}