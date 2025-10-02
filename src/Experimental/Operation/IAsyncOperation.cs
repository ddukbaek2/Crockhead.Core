using Crockhead.Core;


namespace Crockhead.Experimental
{
	/// <summary>
	/// 비동기 명령 인터페이스.
	/// </summary>
	public interface IAsyncOperation : IOperation, IAwaitable<IAwaiter>
	{
	}
}