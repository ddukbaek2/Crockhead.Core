using Crockhead.Core;
using System;


namespace Crockhead.Experimental
{
	/// <summary>
	/// 비동기 오퍼레이션.
	/// </summary>
	public class AsyncOperation : Operation, IAsyncOperation
	{
		/// <summary>
		/// 대기자.
		/// </summary>
		private OperationAwaiter m_Awaiter;

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
			m_Awaiter = new OperationAwaiter(this);
		}

		/// <summary>
		/// 대기자 반환.
		/// </summary>
		IAwaiter IAwaitable<IAwaiter>.GetAwaiter()
		{
			return m_Awaiter;
		}
	}
}
