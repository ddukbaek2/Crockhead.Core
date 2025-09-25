namespace Crockhead.Logging
{
	/// <summary>
	/// 로그 기록 처리자 인터페이스.
	/// </summary>
	public interface ILogWriter
	{
		/// <summary>
		/// 기록.
		/// </summary>
		void Write(ILogger logger, string content);
	}
}