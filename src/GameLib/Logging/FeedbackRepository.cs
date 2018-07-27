using System;
using System.Collections.Generic;

namespace GameLib.Logging
{
	public interface IFeedbackWriter
	{
		void Write(string playerId, string category, string message);
		void WriteSuccess(string playerId, string category, string message);
		void WriteFailure(string playerId, string category, string message);
	}

	public interface IFeedbackRepository : IFeedbackWriter
	{
		List<Feedback> GetById(string playerId);
		void Clear();
	}

	public class FeedbackRepository : IFeedbackRepository
	{
		private readonly Dictionary<string, List<Feedback>> _feedbackMap;
		private readonly ILogger _logger;

		public FeedbackRepository(ILogger logger)
		{
			_logger = logger;
			_feedbackMap = new Dictionary<string, List<Feedback>>();
		}

		public List<Feedback> GetById(string playerId)
		{
			if (_feedbackMap.ContainsKey(playerId))
			{
				return _feedbackMap[playerId];
			}
			return new List<Feedback>();
		}

		public void Write(string playerId, string category, string message)
		{
			WriteMessage(playerId, category, FeedbackType.Information, message);
		}

		public void WriteSuccess(string playerId, string category, string message)
		{
			WriteMessage(playerId, category, FeedbackType.Success, message);
		}

		public void WriteFailure(string playerId, string category, string message)
		{
			WriteMessage(playerId, category, FeedbackType.Failure, message);
		}

		public void Clear()
		{
			_feedbackMap.Clear();
		}

		private void WriteMessage(string playerId, string category, FeedbackType type, string message)
		{
			if (string.IsNullOrEmpty(playerId))
			{
				throw new ArgumentException("Id was null or empty");
			}

			if (_feedbackMap.ContainsKey(playerId) == false)
			{
				_feedbackMap.Add(playerId, new List<Feedback>());
			}
			var feedback = new Feedback { Message = message, Category = category, Type = type };
			_feedbackMap[playerId].Add(feedback);
			_logger.Info($"Added feedback for creature id '{playerId}': " + feedback);
		}
	}
}