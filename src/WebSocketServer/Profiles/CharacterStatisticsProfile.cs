using AutoMapper;

namespace WebSocketServer.Profiles
{
	public class CharacterStatisticsProfile : Profile
	{
		public CharacterStatisticsProfile()
		{
			CreateMap<GameLib.Properties.Stats.CharacterStatistics, Client.Contracts.Properties.Stats.CharacterStatistics>()
			.ForMember(a => a.Race, m => m.MapFrom(s => s.Race))
			.ForMember(a => a.Class, m => m.MapFrom(s => s.Class))
			.ForMember(a => a.Level, m => m.MapFrom(s => s.Level.Total));
		}
	}
}