using Crockhead.Core;


namespace Crockhead.Experimental
{
	/// <summary>
	/// 결과값 있는 비동기 명령 인터페이스.
	/// </summary>
	public interface IAsyncOperation<TResult> : IOperation<TResult>, IAwaitable<IAwaiter<TResult>, TResult>
	{
	}
}