using System;


namespace Crockhead.Logging
{
	/// <summary>
	/// 일지 기록 데이터.
	/// </summary>
	public class JournalEntry : IJournalEntry
	{
		/// <summary>
		/// 기록 일자.
		/// </summary>
		public DateTime DateTime { get; }

		/// <summary>
		/// 수준.
		/// </summary>
		public JournalLevel Level { get; }

		/// <summary>
		/// 내용.
		/// </summary>
		public string Message { get; }

		/// <summary>
		/// 예외.
		/// </summary>
		public Exception Exception { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public JournalEntry(DateTime dateTime, JournalLevel level, string message, Exception exception)
		{
			DateTime = dateTime;
			Level = level;
			Message = message;
			Exception = exception;
		}
	}
}