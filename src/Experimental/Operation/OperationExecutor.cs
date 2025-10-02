using Crockhead.Core;
using System;


namespace Crockhead.Experimental
{
	/// <summary>
	/// 명령으로 처리기.
	/// <para>생성자의 operation 입력 필수.</para>
	/// <para>취소는 operation에서 throw new OperationCanceledException() 호출.</para>
	/// </summary>
	public class OperationExecutor<TOperation> : Disposable, IOperation where TOperation : IOperation, new()
	{
		/// <summary>
		/// 명령.
		/// </summary>
		protected TOperation m_Operation;

		/// <summary>
		/// 액션.
		/// </summary>
		private Action<TOperation> m_Action;

		/// <summary>
		/// 시작 여부 프로퍼티.
		/// </summary>
		public bool IsStarted => m_Operation.IsStarted;

		/// <summary>
		/// 진행 중 여부 프로퍼티.
		/// </summary>
		public bool IsRunning => m_Operation.IsRunning;

		/// <summary>
		/// 완료 여부 프로퍼티. (=Task.IsCompleted)
		/// </summary>
		public bool IsCompleted => m_Operation.IsCompleted;

		/// <summary>
		/// 완료 + 성공 여부 프로퍼티. (=Task.IsCompletedSuccessfully)
		/// </summary>
		public bool IsSucceeded => m_Operation.IsSucceeded;

		/// <summary>
		/// 완료 + 실패 여부 프로퍼티. (=Task.IsFaulted)
		/// </summary>
		public bool IsFaulted => m_Operation.IsFaulted;

		/// <summary>
		/// 완료 + 취소 여부 프로퍼티. (=Task.IsCanceled)
		/// </summary>
		public bool IsCanceled => m_Operation.IsCanceled;

		/// <summary>
		/// 실패 시 예외 프로퍼티.
		/// </summary>
		public Exception Exception => m_Operation.Exception;

		/// <summary>
		/// 상태 프로퍼티.
		/// </summary>
		public OperationStatus Status => m_Operation.Status;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public OperationExecutor(Action<TOperation> operation, Action<TOperation> completion) : base()
		{
			m_Operation = new TOperation();
			m_Action = operation;
			m_Operation.SetCompletion((op) => completion?.Invoke(m_Operation));
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			Disposables.Dispose(m_Operation);
		}

		/// <summary>
		/// 실행.
		/// </summary>
		public TOperation Execute()
		{
			if (m_Operation == null || m_Operation.IsDisposed)
				return default;

			if (m_Operation.IsRunning)
				return m_Operation;

			void OnOperation(IOperation operation)
			{
				try
				{
					operation.Success();
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

		/// <summary>
		/// 명령 설정.
		/// </summary>
		public void SetOperation(Action<IOperation> operation)
		{
			m_Operation.SetOperation(operation);
		}

		/// <summary>
		/// 명령 설정.
		/// </summary>
		public void SetCompletion(Action<IOperation> completion)
		{
			m_Operation.SetCompletion(completion);
		}

		/// <summary>
		/// 재초기화.
		/// </summary>
		public void Reset()
		{
			m_Operation.Reset();
		}

		/// <summary>
		/// 시작.
		/// </summary>
		public void Start()
		{
			m_Operation.Start();
		}

		/// <summary>
		/// 성공.
		/// </summary>
		public void Success()
		{
			m_Operation.Success();
		}

		/// <summary>
		/// 실패.
		/// </summary>
		public void Fail(Exception exception)
		{
			m_Operation.Fail(exception);
		}

		/// <summary>
		/// 취소.
		/// </summary>
		public void Cancel()
		{
			m_Operation.Cancel();
		}
	}
}