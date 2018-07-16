using AutoMapper;

namespace WebSocketServer.Profiles
{
	public class StatisticsProfile : Profile
	{
		public StatisticsProfile()
		{
			CreateMap<GameLib.Properties.Stats.Statistics, Client.Contracts.Properties.Stats.Statistics>()
			.ForMember(a => a.AbilityScores, m => m.MapFrom(s => s.AbilityScores))
			.ForMember(a => a.AttackScores, m => m.MapFrom(s => s.AttackScores))
			.ForMember(a => a.DefenseScores, m => m.MapFrom(s => s.DefenseScores));
		}
	}
}