using System.Runtime.CompilerServices;


namespace Crockhead.Experimental
{
	/// <summary>
	/// 대기 할 수 있는 객체 인터페이스.
	/// <para>await 키워드 사용 가능.</para>
	/// </summary>
	public interface IAwaitable<TAwaiter> where TAwaiter : IAwaiter
	{
		/// <summary>
		/// 대기자 반환.
		/// </summary>
		TAwaiter GetAwaiter();
	}


	/// <summary>
	/// 대기 할 수 있는 객체 인터페이스.
	/// <para>await 키워드 사용 가능.</para>
	/// </summary>
	public interface IAwaitable<TAwaiter, TResult> where TAwaiter : IAwaiter<TResult>
	{
		/// <summary>
		/// 대기자 반환.
		/// </summary>
		TAwaiter GetAwaiter();
	}
}