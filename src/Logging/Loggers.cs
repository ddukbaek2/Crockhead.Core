using System.Collections.Concurrent;


namespace Crockhead.Logging
{
	/// <summary>
	/// 로그 유틸리티.
	/// </summary>
	public static class Loggers
	{
		/// <summary>
		/// 로거 목록.
		/// </summary>
		private static ConcurrentDictionary<string, ILogger> s_Loggers;

		/// <summary>
		/// 생성됨.
		/// </summary>
		static Loggers()
		{
			s_Loggers = new ConcurrentDictionary<string, ILogger>();
		}

		/// <summary>
		/// 로거 생성.
		/// </summary>
		public static TLogger CreateLogger<TLogger>(string name) where TLogger : ILogger, new()
		{
			var logger = new TLogger();
			return logger;
		}

		/// <summary>
		/// 로거 추가.
		/// </summary>
		public static ILogger AddLogger(ILogger logger)
		{
			if (!s_Loggers.TryAdd(logger.Name, logger))
				return null;
			return logger;
		}

		/// <summary>
		/// 로거 제거.
		/// </summary>
		public static ILogger RemoveLogger(string name)
		{
			if (!s_Loggers.TryRemove(name, out var logger))
				return null;
			return logger;
		}

		/// <summary>
		/// 모든 로거 제거.
		/// </summary>
		public static void RemoveAllLoggers()
		{
			s_Loggers.Clear();
		}

		/// <summary>
		/// 로거가 존재 할 경우만 반환.
		/// </summary>
		public static ILogger GetLoggerIfExists(string name)
		{
			if (!s_Loggers.TryGetValue(name, out var logger))
				return null;

			return logger;
		}

		/// <summary>
		/// 로거를 찾아서 반환, 만일 없다면 생성 후 반환.
		/// </summary>
		public static TLogger GetLogger<TLogger>(string name) where TLogger : ILogger, new()
		{
			var logger = Loggers.GetLoggerIfExists(name);
			if (logger == null)
				logger = Loggers.CreateLogger<TLogger>(name);

			return (TLogger)logger;
		}
	}
}