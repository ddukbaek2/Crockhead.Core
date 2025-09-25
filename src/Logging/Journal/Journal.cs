using System;
using System.Collections.Generic;


namespace Crockhead.Logging
{
	/// <summary>
	/// 일지 처리자.
	/// </summary>
	public class Journal : Logger, IJournalLogger
	{
		/// <summary>
		/// 수준 필터.
		/// </summary>
		private List<IJournalFilter> m_Filters;

		/// <summary>
		/// 인터페이스 변환 프로퍼티.
		/// </summary>
		public IJournalLogger AsJournalLogger => (IJournalLogger)this;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Journal(string name, IJournalFilter filter = null, ILogWriter writer = null) : base(name, writer, new JournalFormatter())
		{
			m_Filters = new List<IJournalFilter>();
			AddFilter(filter);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			base.OnDispose(explicitDisposing);
		}

		/// <summary>
		/// 필터 추가.
		/// </summary>
		public void AddFilter(IJournalFilter filter)
		{
			if (filter == null)
				return;
			if (m_Filters.Contains(filter))
				return;

			m_Filters.Add(filter);
		}

		/// <summary>
		/// 필터 제거.
		/// </summary>
		public void RemoveFilter(IJournalFilter filter)
		{
			if (filter == null)
				return;

			if (!m_Filters.Contains(filter))
				return;

			m_Filters.Remove(filter);
		}

		/// <summary>
		/// 필터 전체 제거.
		/// </summary>
		public void RemoveAllFilters()
		{
			m_Filters.Clear();
		}

		/// <summary>
		/// 필터 포함 여부.
		/// </summary>
		public bool ContainsWriter(IJournalFilter filter)
		{
			if (filter == null)
				return false;

			return m_Filters.Contains(filter);
		}

		/// <summary>
		/// 기록.
		/// </summary>
		void ILogger.Log(ILogEntry entry)
		{
			if (entry == null)
				return;

			var journalEntry = entry as IJournalEntry;
			if (journalEntry == null)
				throw new InvalidCastException("entry is not IJournalEntry");

			AsJournalLogger.Log(journalEntry);
		}

		/// <summary>
		/// 기록.
		/// </summary>
		void IJournalLogger.Log(IJournalEntry entry)
		{
			if (entry == null)
				return;

			// 필터 목록 처리.
			for (var i = 0; i < m_Filters.Count; ++i)
			{
				var filter = m_Filters[i];
				if (filter == null)
					continue;

				if (!filter.ShouldLog(this, entry))
					return;
			}

			AsLogger.Log(entry);
		}

		/// <summary>
		/// 추적.
		/// </summary>
		public void Trace(string message)
		{
			var level = JournalLevel.Trace;
			var entry = new JournalEntry(DateTime.Now, level, message, null);
			AsJournalLogger.Log(entry);
		}

		/// <summary>
		/// 디버그.
		/// </summary>
		public void Debug(string message)
		{
			var level = JournalLevel.Debug;
			var entry = new JournalEntry(DateTime.Now, level, message, null);
			AsJournalLogger.Log(entry);
		}

		/// <summary>
		/// 정보.
		/// </summary>
		public void Information(string message)
		{
			var level = JournalLevel.Information;
			var entry = new JournalEntry(DateTime.Now, level, message, null);
			AsJournalLogger.Log(entry);
		}

		/// <summary>
		/// 경고.
		/// </summary>
		public void Warning(string message)
		{
			var level = JournalLevel.Warning;
			var entry = new JournalEntry(DateTime.Now, level, message, null);
			AsJournalLogger.Log(entry);
		}

		/// <summary>
		/// 오류.
		/// </summary>
		public void Error(string message)
		{
			var level = JournalLevel.Error;
			var entry = new JournalEntry(DateTime.Now, level, message, null);
			AsJournalLogger.Log(entry);
		}

		/// <summary>
		/// 치명적인 오류.
		/// </summary>
		public void Critical(string message)
		{
			var level = JournalLevel.Critical;
			var entry = new JournalEntry(DateTime.Now, level, message, null);
			AsJournalLogger.Log(entry);
		}
	}
}