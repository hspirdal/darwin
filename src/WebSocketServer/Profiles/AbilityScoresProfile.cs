using AutoMapper;

namespace WebSocketServer.Profiles
{
	public class AbilityScoresProfile : Profile
	{
		public AbilityScoresProfile()
		{
			CreateMap<GameLib.Properties.Stats.AbilityScores, Client.Contracts.Properties.Stats.AbilityScores>()
			.ForMember(a => a.Strength, m => m.MapFrom(s => s.Strength.Total))
			.ForMember(a => a.Dexterity, m => m.MapFrom(s => s.Dexterity.Total))
			.ForMember(a => a.Constitution, m => m.MapFrom(s => s.Constitution.Total))
			.ForMember(a => a.Intelligence, m => m.MapFrom(s => s.Intelligence.Total))
			.ForMember(a => a.Wisdom, m => m.MapFrom(s => s.Wisdom.Total))
			.ForMember(a => a.Charisma, m => m.MapFrom(s => s.Charisma.Total));
		}
	}
}