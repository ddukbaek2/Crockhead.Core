using System;

namespace Crockhead.Logging
{
	/// <summary>
	/// 기록 처리자 인터페이스.
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// 이름 프로퍼티.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// 사용 여부 프로퍼티.
		/// </summary>
		bool IsEnabled { get; }

		/// <summary>
		/// 사용 여부 설정.
		/// </summary>
		void SetEnable(bool enabled);

		/// <summary>
		/// 형식 지정자 설정.
		/// </summary>
		void SetFormatter(ILogFormatter formatter);

		/// <summary>
		/// 기록 처리자 추가.
		/// </summary>
		void AddWriter(ILogWriter handler);

		/// <summary>
		/// 기록 처리자 제거.
		/// </summary>
		void RemoveWriter(ILogWriter handler);

		/// <summary>
		/// 기록 처리자 전체 제거.
		/// </summary>
		void RemoveAllWriters();

		/// <summary>
		/// 기록 처리자 포함 여부.
		/// </summary>
		bool ContainsWriter(ILogWriter writer);

		/// <summary>
		/// 기록.
		/// </summary>
		void Log(ILogEntry entry);
	}
}