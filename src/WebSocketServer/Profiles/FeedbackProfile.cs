using System.Linq;
using AutoMapper;

namespace WebSocketServer.Profiles
{
	public class FeedbackProfile : Profile
	{
		public FeedbackProfile()
		{
			CreateMap<GameLib.Logging.Feedback, Client.Contracts.Logging.Feedback>();
		}
	}
}