using AutoMapper;

namespace WebSocketServer.Profiles
{
	public class AttackScoresProfile : Profile
	{
		public AttackScoresProfile()
		{
			CreateMap<GameLib.Properties.Stats.AttackScores, Client.Contracts.Properties.Stats.AttackScores>();
		}
	}
}