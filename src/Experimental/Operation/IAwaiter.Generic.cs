using System.Runtime.CompilerServices;


namespace Crockhead.Experimental
{
	/// <summary>
	/// 결과값 있는 대기자 인터페이스.
	/// </summary>
	public interface IAwaiter<TResult> : ICriticalNotifyCompletion
	{
		/// <summary>
		/// 작업 완료 여부 프로퍼티.
		/// <para>참 일 경우 IAwaiter.GetResult() 거짓 일 경우 INotifyCompletion.OnCompleted() ==> continuation ==> IAwaiter.GetResult()</para>
		/// </summary>
		bool IsCompleted { get; }

		/// <summary>
		/// 작업 완료 시 호출됨. (결과값 반환)
		/// </summary>
		TResult GetResult();
	}
}