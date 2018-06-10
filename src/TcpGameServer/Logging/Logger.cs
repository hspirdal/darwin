using System;

namespace TcpGameServer.Logging
{
	public interface ILogger
	{
		void Debug(string message);
		void Info(string message);
		void Warn(string message);
		void Error(Exception exception);
	}

	public class Logger : ILogger
	{
		public void Debug(string message)
		{
			//System.Diagnostics.Debug.WriteLine(message);
			Console.WriteLine(message);
		}

		public void Info(string message)
		{
			Console.WriteLine(message);
		}

		public void Warn(string message)
		{
			Console.WriteLine($"WARNING - {message}");
		}

		public void Error(Exception exception)
		{
			var innermostException = TraverseInnermostException(exception);
			Console.WriteLine($"Exception - {innermostException.Message}");
			Console.WriteLine(innermostException.StackTrace);
		}

		private static Exception TraverseInnermostException(Exception exception)
		{
			if (exception.InnerException != null)
			{
				return TraverseInnermostException(exception);
			}
			return exception;
		}
	}
}