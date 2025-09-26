namespace Crockhead.Logging
{
	/// <summary>
	/// 일지 필터 인터페이스.
	/// <para>참을 반환할 경우에만 기록.</para>
	/// </summary>
	public interface IJournalFilter
	{
		/// <summary>
		/// 엔트리를 로그 해야하는지 여부.
		/// </summary>
		bool ShouldLog(IJournalLogger logger, IJournalEntry entry);
	}
}
