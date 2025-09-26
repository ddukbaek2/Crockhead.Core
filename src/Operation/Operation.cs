using System;
using System.Threading;


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
		/// 진행 중 프로퍼티.
		/// </summary>
		public bool IsRunning => m_IsStarted && !m_IsCompleted;

		/// <summary>
		/// 시작 여부 프로퍼티.
		/// <para>한번이라도 Start()를 호출하면 이후 Reset() 전까지는 계속 참을 반환.</para>
		/// </summary>
		public bool IsStarted => m_IsStarted; // && !m_IsCompleted

		/// <summary>
		/// 완료 여부 프로퍼티.
		/// <para>한번이라도 Complete() or Cancel()을 호출하면 이후 Reset() 전까지는 계속 참을 반환.</para>
		/// </summary>
		public bool IsCompleted => m_IsCompleted;

		/// <summary>
		/// 완료 + 성공 여부 프로퍼티.
		/// </summary>
		public bool IsSucceeded => IsCompleted && m_IsSucceeded;

		/// <summary>
		/// 완료 + 실패 여부 프로퍼티.
		/// </summary>
		public bool IsFaulted => IsCompleted && !m_IsSucceeded;

		/// <summary>
		/// 완료 + 취소 여부 프로퍼티.
		/// </summary>
		public bool IsCanceled => IsCompleted && m_IsCancelled;

		/// <summary>
		/// 실패 예외 프로퍼티.
		/// </summary>
		public Exception Exception => m_Exception;

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
			m_IsStarted = false;
			m_IsCompleted = false;
			m_IsSucceeded = false;
			m_IsCancelled = false;
			m_Exception = null;

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
		/// 명령 재사용 할 수 있도록 초기화.
		/// </summary>
		public virtual void Reset()
		{
			// 명령 실행 중에는 변경 불가.
			if (IsRunning)
				return;

			m_IsStarted = false;
			m_IsCompleted = false;
			m_IsSucceeded = false;
			m_IsCancelled = false;
			m_Exception = null;
		}

		/// <summary>
		/// 명령 설정.
		/// </summary>
		public virtual void SetOperation(Action<IOperation> operation)
		{
			// 명령 실행 중에는 변경 불가.
			if (IsRunning)
				return;

			m_Operation = operation;
		}

		/// <summary>
		/// 명령 설정.
		/// </summary>
		public virtual void SetCompletion(Action<IOperation> completion)
		{
			// 명령 실행 중에는 변경 불가.
			if (IsRunning)
				return;

			m_Completion = completion;
		}

		/// <summary>
		/// 명령 시작.
		/// </summary>
		public virtual void Start()
		{
			// 명령 시작 된 후에는 호출 불가.
			//if (m_IsStarted || m_IsCompleted)

			// Reset() 없이는 Start()하면 다시는 m_IsStarted가 false가 될 일이 없음.
			if (m_IsStarted)
				return;

			m_IsStarted = true;
			try
			{
				m_Operation?.Invoke(this);
			}
			catch (Exception exception)
			{
				Fail(exception);
				throw;
			}
		}

		/// <summary>
		/// 대기.
		/// </summary>
		public virtual void WaitForCompletion()
		{
			if (!m_IsStarted)
				return;

			while (!m_IsCompleted)
			{
				Thread.Sleep(1);
			}
		}

		/// <summary>
		/// 명령 완료.
		/// </summary>
		protected void Complete(bool succeeded)
		{
			// 명령이 시작되지 않았거나, 명령이 완료된 후에는 호출 불가.
			if (!m_IsStarted || m_IsCompleted)
				return;

			m_IsCompleted = true;
			m_IsSucceeded = succeeded;

			try
			{
				m_Completion?.Invoke(this);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 명령 성공.
		/// </summary>
		public virtual void Success()
		{
			// 명령이 시작되지 않았거나, 명령이 완료된 후에는 호출 불가.
			if (!m_IsStarted || m_IsCompleted)
				return;

			Complete(true);
		}

		/// <summary>
		/// 명령 실패.
		/// </summary>
		public virtual void Fail(Exception exception)
		{
			// 명령이 시작되지 않았거나, 명령이 완료된 후에는 호출 불가.
			if (!m_IsStarted || m_IsCompleted)
				return;

			m_Exception = exception;
			Complete(false);
		}

		/// <summary>
		/// 명령 취소.
		/// </summary>
		public virtual void Cancel()
		{
			// 명령이 시작되지 않았거나, 명령이 완료된 후에는 호출 불가.
			if (!m_IsStarted || m_IsCompleted)
				return;

			m_IsCancelled = true;
			Complete(false);
		}
	}
}