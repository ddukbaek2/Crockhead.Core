using Crockhead.Core;
using System;


namespace Crockhead.Experimental
{
	/// <summary>
	/// 명령으로 결과를 얻어오는 처리기.
	/// <para>생성자의 operation 입력 필수.</para>
	/// <para>취소는 operation에서 throw new OperationCanceledException() 호출.</para>
	/// </summary>
	public class OperationExecutor<TOperation, TResult> : OperationExecutor<TOperation>, IOperation<TResult> where TOperation : IOperation<TResult>, new()
	{
		/// <summary>
		/// 결과.
		/// </summary>
		public TResult Result => m_Operation.Result;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public OperationExecutor(Action<TOperation> operation, Action<TOperation> completion) : base(operation, completion)
		{
		}

		/// <summary>
		/// 실행.
		/// </summary>
		public new TOperation Execute()
		{
			if (m_Operation == null || m_Operation.IsDisposed)
				return default;

			if (m_Operation.IsRunning)
				return m_Operation;

			void OnOperation(IOperation operation)
			{
				try
				{
					var result = m_Operation.Result;
					m_Operation.Success(result);
				}
				catch (OperationCanceledException)
				{
					m_Operation.Cancel();
				}
				catch (Exception exception)
				{
					m_Operation.Fail(exception);
					throw;
				}
			}

			m_Operation.SetOperation(OnOperation);
			m_Operation.Start();
			return m_Operation;
		}

		/// <summary>
		/// 성공.
		/// </summary>
		public void Success(TResult result)
		{
			m_Operation.Success(result);
		}
	}
}