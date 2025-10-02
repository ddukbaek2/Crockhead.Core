using Crockhead.Core;
using System;


namespace Crockhead.Experimental
{
	/// <summary>
	/// 결과값 있는 비동기 오퍼레이션.
	/// </summary>
	public class AsyncOperation<TResult> : Operation<TResult>, IAsyncOperation<TResult>
	{
		/// <summary>
		/// 대기자.
		/// </summary>
		private OperationAwaiter<TResult> m_Awaiter;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public AsyncOperation() : this(null, null)
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public AsyncOperation(Action<IOperation> operation) : this(operation, null)
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public AsyncOperation(Action<IOperation> operation, Action<IOperation> completion) : base(operation, completion)
		{
			m_Awaiter = new OperationAwaiter<TResult>(this);
		}

		/// <summary>
		/// 대기자 반환.
		/// </summary>
		IAwaiter<TResult> IAwaitable<IAwaiter<TResult>, TResult>.GetAwaiter()
		{
			return m_Awaiter;
		}
	}
}
