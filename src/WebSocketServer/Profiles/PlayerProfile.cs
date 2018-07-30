using AutoMapper;
using GameLib.Properties.Stats;

namespace WebSocketServer.Profiles
{
	public class PlayerProfile : Profile
	{
		public PlayerProfile()
		{
			CreateMap<GameLib.Entities.Player, Client.Contracts.Entities.Player>()
			.ForMember(i => i.Level, x => x.MapFrom(i => i.Level.Total))
			.ForMember(i => i.Healthiness, x => x.MapFrom(i => HealthinessReader.Measure(i.Statistics.DefenseScores.HitPoints)));
		}
	}
}