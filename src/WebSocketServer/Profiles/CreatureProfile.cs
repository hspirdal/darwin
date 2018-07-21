using AutoMapper;

namespace WebSocketServer.Profiles
{
	public class CreatureProfile : Profile
	{
		public CreatureProfile()
		{
			CreateMap<GameLib.Entities.Creature, Client.Contracts.Entities.Creature>();
		}
	}
}