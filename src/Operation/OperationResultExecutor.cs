using System;


namespace Crockhead.Core
{
	/// <summary>
	/// 명령으로 결과를 얻어오는 처리기.
	/// <para>생성자의 operation 입력 필수.</para>
	/// <para>취소는 operation에서 throw new OperationCanceledException() 호출.</para>
	/// </summary>
	public class OperationResultExecutor<TResult> : Disposable
	{
		/// <summary>
		/// 명령.
		/// </summary>
		private Operation<TResult> m_Operation;

		/// <summary>
		/// 결과 생성기.
		/// </summary>
		private Func<TResult> m_ResultGetter;

		/// <summary>
		/// 명령 프로퍼티.
		/// </summary>
		public Operation<TResult> Operation => m_Operation;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public OperationResultExecutor(Func<TResult> operation) : base()
		{
			m_Operation = new Operation<TResult>();
			m_ResultGetter = operation;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			Disposables.Dispose(m_Operation);
			m_Operation = null;
		}

		/// <summary>
		/// 실행.
		/// </summary>
		public Operation<TResult> Execute()
		{
			if (m_Operation == null || m_Operation.IsDisposed)
				return null;

			if (m_Operation.IsRunning)
				return m_Operation;

			if (m_ResultGetter == null)
				throw new InvalidOperationException("NullResultGettingOperation.");

			void OnOperation(Operation<TResult> operation)
			{
				try
				{
					var result = m_ResultGetter.Invoke();
					operation.Success(result);
				}
				catch (OperationCanceledException)
				{
					operation.Cancel();
				}
				catch (Exception exception)
				{
					operation.Fail(exception);
					throw;
				}
			}

			m_Operation.SetOperation(OnOperation);
			m_Operation.Start();
			return m_Operation;
		}
	}
}