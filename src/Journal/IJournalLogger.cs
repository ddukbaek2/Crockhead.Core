namespace Crockhead.Logging
{
	/// <summary>
	/// 일지 인터페이스.
	/// </summary>
	public interface IJournalLogger : ILogger
	{
		/// <summary>
		/// 필터 추가.
		/// </summary>
		void AddFilter(IJournalFilter filter);

		/// <summary>
		/// 필터 제거.
		/// </summary>
		void RemoveFilter(IJournalFilter filter);

		/// <summary>
		/// 필터 전체 제거.
		/// </summary>
		void RemoveAllFilters();

		/// <summary>
		/// 필터 포함 여부.
		/// </summary>
		bool ContainsWriter(IJournalFilter filter);

		/// <summary>
		/// 기록.
		/// </summary>
		void Log(IJournalEntry entry);
	}
}