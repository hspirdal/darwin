using AutoMapper;

namespace WebSocketServer.Profiles
{
	public class DefenseScoresProfile : Profile
	{
		public DefenseScoresProfile()
		{
			CreateMap<GameLib.Properties.Stats.DefenseScores, Client.Contracts.Properties.Stats.DefenseScores>()
			.ForMember(a => a.ArmorClass, m => m.MapFrom(s => s.ArmorClass.Total))
			.ForMember(a => a.HitPoints, m => m.MapFrom(s => s.HitPoints.Total));
		}
	}
}