using System.Runtime.CompilerServices;


namespace Crockhead.Experimental
{
	/// <summary>
	/// 대기자 인터페이스.
	/// <para>실제적인 대기 기능을 DotNet 명세에 따라 구현.</para>
	/// </summary>
	public interface IAwaiter : ICriticalNotifyCompletion
	{
		/// <summary>
		/// 작업 완료 여부 프로퍼티.
		/// <para>참 일 경우 IAwaiter.GetResult() 거짓 일 경우 INotifyCompletion.OnCompleted() ==> continuation ==> IAwaiter.GetResult()</para>
		/// </summary>
		bool IsCompleted { get; }

		/// <summary>
		/// 작업 완료 시 호출됨. (결과값 없음)
		/// </summary>
		void GetResult();
	}
}