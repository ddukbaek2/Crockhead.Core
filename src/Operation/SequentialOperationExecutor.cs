using System;
using System.Collections.Generic;


namespace Crockhead.Core
{
	/// <summary>
	/// 순차적 명령 실행기.
	/// <para>명령을 실행할 때 기존 명령이 있다면 내부적으로 큐에 쌓아 보류(Pending)시키고 순서대로 하나씩 순차적(Sequential)으로 처리.</para>
	/// </summary>
	public class SequentialOperationExecutor
	{
		/// <summary>
		/// 보류 중인 명령 목록.
		/// </summary>
		private Queue<Operation> m_PendingQueue;

		/// <summary>
		/// 재진입 방어.
		/// </summary>
		private bool m_IsContinuing;

		/// <summary>
		/// 진행 여부 프로퍼티.
		/// </summary>
		public bool IsRunning => m_PendingQueue.Count > 0;

		/// <summary>
		/// 현재 오퍼레이션.
		/// </summary>
		public Operation Current => IsRunning ? m_PendingQueue.Peek() : null;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SequentialOperationExecutor()
		{
			m_PendingQueue = new Queue<Operation>();
			m_IsContinuing = false;
		}

		/// <summary>
		/// 모든 작업 중단하고 큐 비우기.
		/// </summary>
		public void Clear()
		{
			if (m_PendingQueue.Count == 0)
				return;

			var current = Current;
			m_PendingQueue.Clear();
			if (current.IsRunning)
				current.Cancel();
		}

		/// <summary>
		/// 현재 명령 취소.
		/// </summary>
		public void Cancel()
		{
			if (m_PendingQueue.Count == 0)
				return;

			Current.Cancel();
		}

		/// <summary>
		/// 명령 추가.
		/// </summary>
		public void Execute(Action action)
		{
			if (action == null)
				return;

			void OnOperation(Operation operation)
			{
				try
				{
					action?.Invoke();
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

			Execute(OnOperation);
		}

		/// <summary>
		/// 명령 실행.
		/// </summary>
		public void Execute(Action<Operation> operation)
		{
			if (operation == null)
				return;

			void OnCompletion(Operation operation)
			{
				// 비동기 작업이 완료되면 이어서 명령 처리 호출.
				Continue();
			}

			m_PendingQueue.Enqueue(new Operation(operation, OnCompletion));
			Continue();
		}

		/// <summary>
		/// 이어서 명령 처리.
		/// </summary>
		private void Continue()
		{
			if (m_IsContinuing)
				return;

			m_IsContinuing = true;
			try
			{
				while (m_PendingQueue.Count > 0)
				{
					var operation = m_PendingQueue.Peek();
					
					// 시작되지 않았을 때.
					if (!operation.IsStarted)
					{
						operation.Start();

						// 즉시 완료시.
						if (operation.IsCompleted)
						{
							m_PendingQueue.Dequeue();
							continue;
						}

						// 현재 프레임에 완료가 되지 않는 경우.
						return;
					}

					// 완료시.
					if (operation.IsCompleted)
					{
						m_PendingQueue.Dequeue();
						continue;
					}

					// 현재 프레임에 완료가 되지 않는 경우.
					return;
				}
			}
			finally
			{
				m_IsContinuing = false;
			}
		}
	}
}