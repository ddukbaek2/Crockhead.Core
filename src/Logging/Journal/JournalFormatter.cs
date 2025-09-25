using System;


namespace Crockhead.Logging
{
	/// <summary>
	/// 기본 일지 형식 지정자.
	/// </summary>
	public class JournalFormatter : ILogFormatter
	{
		/// <summary>
		/// 양식에 맞춘 문자열로 변환.
		/// </summary>
		string ILogFormatter.Format(ILogger logger, ILogEntry entry)
		{
			var journal = logger as IJournalLogger;
			if (journal == null)
				throw new InvalidCastException("logger is not IJournalLogger");

			var journalEntry = entry as IJournalEntry;
			if (journalEntry == null)
				throw new InvalidCastException("entry is not IJournalEntry");

			var formattedMessage = Format(journal, journalEntry);
			return formattedMessage;
		}

		/// <summary>
		/// 일지 양식에 맞춘 문자열로 변환.
		/// </summary>
		public string Format(IJournalLogger journal, IJournalEntry entry)
		{
			var timestamp = entry.DateTime.ToString("yyyy-MM-dd HH:mm:ss:FFF");
			var name = journal.Name;

			if (entry.Exception == null)
			{
				return $"[{timestamp}][{entry.Level}][{name}] {entry.Message}";
			}
			else
			{
				return $"[{timestamp}][{entry.Level}][{name}] {entry.Exception}";
			}
		}
	}
}