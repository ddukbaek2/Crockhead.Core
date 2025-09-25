namespace Crockhead.Logging
{
	/// <summary>
	/// 일지 수준 필터.
	/// </summary>
	public class JournalLevelFilter : IJournalFilter
	{
		/// <summary>
		/// 허용 레벨.
		/// <para>이넘플래그로 여러개 결합 가능.</para>
		/// </summary>
		public JournalLevel AllowLevel { get; set; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public JournalLevelFilter(JournalLevel allowLevel)
		{
			AllowLevel = allowLevel;
		}

		/// <summary>
		/// 엔트리를 로그 해야하는지 여부.
		/// </summary>
		bool IJournalFilter.ShouldLog(IJournalLogger journal, IJournalEntry entry)
		{
			// 엔트리의 수준이 허용 수준에 포함되어있음.
			var allowed = ((AllowLevel & entry.Level) != 0);
			return allowed;
		}
	}
}