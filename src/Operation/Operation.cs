using System;


namespace Crockhead.Core
{
	/// <summary>
	/// 명령.
	/// <para>수동으로 Start / Complete / Cancel 호출 필요.</para>
	/// <para>명령 콜백으로부터 실제 처리할 작업을 하고, 끝나면 직접 Operation의 Complete() or Cancel() 호출하여 작업 완료 처리 필수.</para>
	/// <para>취소의 경우 즉시 명령이 실행되고 동일 스택에서 결과까지 반영되는 동기적 사용시 쓸 수 없음.</para>
	/// <para>operation에서 OperationCanceledException 해도 취소 가능.</para>
	/// </summary>
	public class Operation : Disposable, IOperation
	{
		/// <summary>
		/// 명령 처리 콜백.
		/// </summary>
		private Action<IOperation> m_Operation;

		/// <summary>
		/// 명령 완료 콜백.
		/// </summary>
		private Action<IOperation> m_Completion;

		/// <summary>
		/// 시작 여부.
		/// </summary>
		private bool m_IsStarted;

		/// <summary>
		/// 완료 여부.
		/// </summary>
		private bool m_IsCompleted;

		/// <summary>
		/// 성공 여부.
		/// </summary>
		private bool m_IsSucceeded;

		/// <summary>
		/// 취소 여부.
		/// </summary>
		private bool m_IsCancelled;

		/// <summary>
		/// 실패 예외.
		/// </summary>
		Exception m_Exception;

		/// <summary>
		/// 시작 여부 프로퍼티.
		/// </summary>
		public bool IsStarted => m_IsStarted; // && !m_IsCompleted

		/// <summary>
		/// 진행 중 여부 프로퍼티.
		/// </summary>
		public bool IsRunning => m_IsStarted && !m_IsCompleted;

		/// <summary>
		/// 완료 여부 프로퍼티. (=Task.IsCompleted)
		/// </summary>
		public bool IsCompleted => m_IsCompleted;

		/// <summary>
		/// 완료 + 성공 여부 프로퍼티. (=Task.IsCompletedSuccessfully)
		/// </summary>
		public bool IsSucceeded => IsCompleted && m_IsSucceeded;

		/// <summary>
		/// 완료 + 실패 여부 프로퍼티. (=Task.IsFaulted)
		/// </summary>
		public bool IsFaulted => IsCompleted && !m_IsSucceeded;

		/// <summary>
		/// 완료 + 취소 여부 프로퍼티. (=Task.IsCanceled)
		/// </summary>
		public bool IsCanceled => IsCompleted && m_IsCancelled;

		/// <summary>
		/// 실패 시 예외 프로퍼티.
		/// </summary>
		public Exception Exception => m_Exception;

		/// <summary>
		/// 상태 프로퍼티.
		/// </summary>
		public OperationStatus Status
		{
			get
			{
				if (!m_IsStarted)
					return OperationStatus.Wait;
				else if (!m_IsCompleted)
					return OperationStatus.Running;
				else if (m_IsCancelled)
					return OperationStatus.Canceled;
				else if (m_IsSucceeded)
					return OperationStatus.Succeeded;
				else
					return OperationStatus.Faulted;
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Operation() : this(null, null)
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Operation(Action<IOperation> operation) : this(operation, null)
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Operation(Action<IOperation> operation, Action<IOperation> completion) : base()
		{
			OnReset();
			SetOperation(operation);
			SetCompletion(completion);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			m_Operation = null;
			m_Completion = null;
		}

		/// <summary>
		/// 재초기화됨.
		/// </summary>
		protected virtual void OnReset()
		{
			m_IsStarted = false;
			m_IsCompleted = false;
			m_IsSucceeded = false;
			m_IsCancelled = false;
			m_Exception = null;
		}

		/// <summary>
		/// 시작됨.
		/// </summary>
		protected virtual void OnStarted()
		{
			m_Operation?.Invoke(this);
		}

		/// <summary>
		/// 완료됨.
		/// </summary>
		protected virtual void OnCompleted(bool succeeded)
		{
			m_Completion?.Invoke(this);
		}

		/// <summary>
		/// 명령 설정.
		/// </summary>
		public void SetOperation(Action<IOperation> operation)
		{
			// 명령 실행 중에는 변경 불가.
			if (IsRunning)
				return;

			m_Operation = operation;
		}

		/// <summary>
		/// 명령 설정.
		/// </summary>
		public void SetCompletion(Action<IOperation> completion)
		{
			// 명령 실행 중에는 변경 불가.
			if (IsRunning)
				return;

			m_Completion = completion;
		}

		/// <summary>
		/// 재초기화.
		/// </summary>
		public void Reset()
		{
			// 명령 실행 중에는 변경 불가.
			if (IsRunning)
				return;

			OnReset();
		}

		/// <summary>
		/// 시작.
		/// </summary>
		public void Start()
		{
			// 명령 시작 된 후에는 호출 불가.
			if (m_IsStarted)
				return;

			m_IsStarted = true;

			try
			{
				OnStarted();
			}
			catch (Exception exception)
			{
				Fail(exception);
				throw;
			}
		}

		/// <summary>
		/// 완료.
		/// </summary>
		private void Complete(bool succeeded)
		{
			// 명령이 시작되지 않았거나, 명령이 완료된 후에는 호출 불가.
			if (!m_IsStarted || m_IsCompleted)
				return;

			m_IsCompleted = true;
			m_IsSucceeded = succeeded;

			try
			{
				OnCompleted(succeeded);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 성공.
		/// </summary>
		public void Success()
		{
			// 명령이 시작되지 않았거나, 명령이 완료된 후에는 호출 불가.
			if (!m_IsStarted || m_IsCompleted)
				return;

			Complete(true);
		}

		/// <summary>
		/// 실패.
		/// </summary>
		public void Fail(Exception exception)
		{
			// 명령이 시작되지 않았거나, 명령이 완료된 후에는 호출 불가.
			if (!m_IsStarted || m_IsCompleted)
				return;

			m_Exception = exception;
			Complete(false);
		}

		/// <summary>
		/// 취소.
		/// </summary>
		public void Cancel()
		{
			// 명령이 시작되지 않았거나, 명령이 완료된 후에는 호출 불가.
			if (!m_IsStarted || m_IsCompleted)
				return;

			m_IsCancelled = true;
			Complete(false);
		}
	}
}