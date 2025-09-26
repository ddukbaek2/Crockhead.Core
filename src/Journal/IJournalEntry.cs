using System;


namespace Crockhead.Logging
{
	/// <summary>
	/// 일지 기록 데이터 인터페이스
	/// </summary>
	public interface IJournalEntry : ILogEntry
	{
		/// <summary>
		/// 수준.
		/// </summary>
		JournalLevel Level { get; }
	}
}