namespace Crockhead.Logging
{
	/// <summary>
	/// 로그 형식 지정자 인터페이스.
	/// </summary>
	public interface ILogFormatter
	{
		/// <summary>
		/// 양식에 맞춘 문자열로 변환.
		/// </summary>
		string Format(ILogger logger, ILogEntry entry);
	}
}