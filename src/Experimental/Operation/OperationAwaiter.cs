using Crockhead.Core;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;


namespace Crockhead.Experimental
{
	/// <summary>
	/// 명령 대기자.
	/// </summary>
	public readonly struct OperationAwaiter : IAwaiter
	{
		/// <summary>
		/// 명령.
		/// </summary>
		private readonly IAsyncOperation m_Operation;

		/// <summary>
		/// 완료 여부 프로퍼티.
		/// </summary>
		bool IAwaiter.IsCompleted => m_Operation.IsCompleted;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public OperationAwaiter(IAsyncOperation operation)
		{
			if (operation == null)
				throw new ArgumentNullException(nameof(operation));

			m_Operation = operation;
		}

		/// <summary>
		/// await 키워드에서 호출됨.
		/// </summary>
		void INotifyCompletion.OnCompleted(Action continuation)
		{
			if (continuation == null)
				return;

			if (m_Operation.IsCompleted)
			{
				continuation();
			}
			else
			{
				m_Operation.SetCompletion((operation) => continuation());
			}
		}

		/// <summary>
		/// await 키워드에서 호출됨.
		/// </summary>
		void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation)
		{
			((INotifyCompletion)this).OnCompleted(continuation);
		}

		/// <summary>
		/// 결과 처리.
		/// </summary>
		void IAwaiter.GetResult()
		{
			if (m_Operation.Exception != null)
			{
				var exceptionDispatchInfo = ExceptionDispatchInfo.Capture(m_Operation.Exception);
				exceptionDispatchInfo.Throw();
			}
		}
	}
}