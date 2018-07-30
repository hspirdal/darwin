using AutoMapper;
using GameLib.Properties.Stats;

namespace WebSocketServer.Profiles
{
	public class CreatureProfile : Profile
	{
		public CreatureProfile()
		{
			CreateMap<GameLib.Entities.Creature, Client.Contracts.Entities.Creature>()
			.ForMember(i => i.Healthiness, x => x.MapFrom(i => HealthinessReader.Measure(i.Statistics.DefenseScores.HitPoints)));
		}
	}
}