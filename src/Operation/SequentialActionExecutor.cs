using System;
using System.Collections.Generic;


namespace Crockhead.Core
{
	/// <summary>
	/// 순차적 액션 실행기.
	/// <para>액션을 실행할 때 기존 액션이 있다면 내부적으로 큐에 쌓아 보류(Pending)시키고 순서대로 하나씩 순차적(Sequential)으로 처리.</para>
	/// <para>성공/실패 개념은 없음.</para>
	/// </summary>
	public class SequentialActionExecutor : Disposable
	{
		/// <summary>
		/// 동작 중인지 여부.
		/// </summary>
		private bool m_IsRunning;

		/// <summary>
		/// 보류 중인 명령 목록.
		/// </summary>
		private Queue<Action<Action>> m_PendingQueue;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SequentialActionExecutor() : base()
		{
			m_IsRunning = false;
			m_PendingQueue = new Queue<Action<Action>>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 명령 실행.
		/// </summary>
		public void Execute(Action action)
		{
			if (action == null)
				return;

			void OnOperation(Action complete)
			{
				try
				{
					action.Invoke();
				}
				finally
				{
					complete.Invoke();
				}
			}

			// 비동기 실행.
			Execute(OnOperation);
		}

		/// <summary>
		/// 비동기적 명령 실행.
		/// <para>실행 후 수동 종료 함수 호출 필요.</para>
		/// </summary>
		public void Execute(Action<Action> operation)
		{
			if (operation == null)
				return;

			m_PendingQueue.Enqueue(operation);
			BeginOperation();
		}

		/// <summary>
		/// 명령 처리 시작.
		/// <para>현재 명령이 처리 중이 아닐 경우, 쌓여있는 다음 명령 수행을 시도함.</para>
		/// </summary>
		private void BeginOperation()
		{
			if (m_IsRunning || m_PendingQueue.Count == 0)
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
				var operation = m_PendingQueue.Dequeue();
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