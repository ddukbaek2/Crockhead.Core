using System;


namespace Crockhead.Logging
{
	/// <summary>
	/// 로그 기록 데이터 인터페이스.
	/// </summary>
	public interface ILogEntry
	{
		/// <summary>
		/// 기록 일자.
		/// </summary>
		DateTime DateTime { get; }

		/// <summary>
		/// 내용.
		/// </summary>
		string Message { get; }

		/// <summary>
		/// 예외.
		/// </summary>
		Exception Exception { get; }
	}
}