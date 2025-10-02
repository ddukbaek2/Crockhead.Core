using System;

namespace Crockhead.Core
{
	/// <summary>
	/// 명령 인터페이스.
	/// </summary>
	public interface IAsyncOperation : IOperation
	{
		/// <summary>
		/// 대기.
		/// </summary>
		void WaitForCompletion();
	}
}