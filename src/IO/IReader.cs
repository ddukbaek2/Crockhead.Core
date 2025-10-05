namespace Crockhead.Core
{
	/// <summary>
	/// 리더.
	/// </summary>
	public interface IReader<TResult>
	{
		/// <summary>
		/// 읽은 결과.
		/// </summary>
		TResult Result { get; }

		/// <summary>
		/// 읽기.
		/// </summary>
		bool Read();
	}
}