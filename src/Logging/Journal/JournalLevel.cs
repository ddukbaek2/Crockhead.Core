using System;


namespace Crockhead.Logging
{
	/// <summary>
	/// 일지 수준.
	/// </summary>
	[Flags]
	public enum JournalLevel
	{
		/// <summary>
		/// 없음.
		/// </summary>
		None = 0,

		/// <summary>
		/// 추적.
		/// </summary>
		Trace = 1 << 0,

		/// <summary>
		/// 디버그.
		/// </summary>
		Debug = 1 << 1,

		/// <summary>
		/// 정보.
		/// </summary>
		Information = 1 << 2,

		/// <summary>
		/// 경고.
		/// </summary>
		Warning = 1 << 3,

		/// <summary>
		/// 오류.
		/// <para>기능적인 실패.</para>
		/// </summary>
		Error = 1 << 4,

		/// <summary>
		/// 심각한 오류.
		/// <para>주요 시스템 기능 실패.</para>
		/// </summary>
		Critical = 1 << 5,

		/// <summary>
		/// 치명적인 오류.
		/// <para>애플리케이션 작동을 위한 필수 기능 실패.</para>
		/// </summary>
		Fatal = 1 << 6,

		/// <summary>
		/// 전부.
		/// </summary>
		All = Trace | Debug | Information | Warning | Error | Critical | Fatal,
	}
}