using System.Collections.Generic;


namespace Crockhead.Core
{
	/// <summary>
	/// 순차적 명령 처리기.
	/// <para>명령을 실행할 때 기존 명령이 있다면 내부적으로 큐에 쌓아 보류(Pending)시키고 기존 명령대로 하나씩 순차적(Sequential)으로 처리.</para>
	/// </summary>
	public class SequentialOperator
	{
		/// <summary>
		/// 명령 시작 델리게이트.
		/// </summary>
		public delegate void StartOperationDelegate();

		/// <summary>
		/// 명령 시작 + 명령 종료 델리게이트.
		/// </summary>
		public delegate void StartWithFinishOperationDelegate(FinishOperationDelegate finish);

		/// <summary>
		/// 명령 종료 델리게이트.
		/// </summary>
		public delegate void FinishOperationDelegate();

		/// <summary>
		/// 동작 중인지 여부.
		/// </summary>
		private bool m_IsRunning;

		/// <summary>
		/// 보류 중인 명령 목록.
		/// </summary>
		private Queue<StartWithFinishOperationDelegate> m_PendingOperations;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SequentialOperator()
		{
			m_IsRunning = false;
			m_PendingOperations = new Queue<StartWithFinishOperationDelegate>();
		}

		/// <summary>
		/// 보류 중인 명령 모두 제거.
		/// <para>현재 명령은 중단되지 않음.</para>
		/// </summary>
		public void RemoveAllPendingOperations()
		{
			m_PendingOperations.Clear();
		}

		/// <summary>
		/// 명령 실행.
		/// </summary>
		public void Execute(StartOperationDelegate operation)
		{
			if (operation == null)
				return;

			// 비동기 실행.
			Execute((finish) =>
			{
				try
				{
					operation.Invoke();
				}
				finally
				{
					finish.Invoke();
				}
			});
		}

		/// <summary>
		/// 비동기적 명령 실행.
		/// <para>실행 후 수동 종료 함수 호출 필요.</para>
		/// </summary>
		public void Execute(StartWithFinishOperationDelegate operation)
		{
			if (operation == null)
				return;
			m_PendingOperations.Enqueue(operation);
			BeginOperation();
		}

		/// <summary>
		/// 명령 처리 시작.
		/// <para>현재 명령이 처리 중이 아닐 경우, 쌓여있는 다음 명령 수행을 시도함.</para>
		/// </summary>
		private void BeginOperation()
		{
			if (m_IsRunning || m_PendingOperations.Count == 0)
				return;

			var finished = false;
			void FinishOnce()
			{
				if (finished)
					return;

				finished = true;
				EndOperation();
			}

			try
			{
				m_IsRunning = true;
				var operation = m_PendingOperations.Dequeue();
				operation?.Invoke(FinishOnce);
			}
			catch// (Exception exception)
			{
				EndOperation();
			}
		}

		/// <summary>
		/// 명령 처리 종료.
		/// </summary>
		private void EndOperation()
		{
			if (!m_IsRunning)
				return;

			m_IsRunning = false;
			BeginOperation();
		}
	}
}