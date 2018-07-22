using System;
using System.Collections.Generic;

namespace GameLib.Logging
{
	public interface IFeedbackRepository
	{
		List<string> GetById(string playerId);
		void WriteLine(string playerId, string message);
		void Clear();
	}

	public interface IFeedbackWriter
	{
		void WriteLine(string playerId, string message);
	}

	public class FeedbackRepository : IFeedbackWriter, IFeedbackRepository
	{
		private readonly Dictionary<string, List<string>> _feedbackMap;

		public FeedbackRepository()
		{
			_feedbackMap = new Dictionary<string, List<string>>();
		}

		public List<string> GetById(string playerId)
		{
			if (_feedbackMap.ContainsKey(playerId))
			{
				return _feedbackMap[playerId];
			}
			return new List<string>();
		}

		public void WriteLine(string playerId, string message)
		{
			if (string.IsNullOrEmpty(playerId))
			{
				throw new ArgumentException("Id was null or empty");
			}

			if (_feedbackMap.ContainsKey(playerId) == false)
			{
				_feedbackMap.Add(playerId, new List<string>());
			}
			_feedbackMap[playerId].Add(message);
		}

		public void Clear()
		{
			_feedbackMap.Clear();
		}
	}
}