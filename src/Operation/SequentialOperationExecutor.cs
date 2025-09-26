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
		private Queue<IOperation> m_PendingQueue;

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
		public IOperation Current => IsRunning ? m_PendingQueue.Peek() : null;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public SequentialOperationExecutor()
		{
			m_PendingQueue = new Queue<IOperation>();
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
		public IOperation Execute(Action action)
		{
			if (action == null)
				return null;

			void OnOperation(IOperation operation)
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

			var newOperation = Execute(OnOperation);
			return newOperation;
		}

		/// <summary>
		/// 명령 실행.
		/// </summary>
		public IOperation Execute(Action<IOperation> operation)
		{
			if (operation == null)
				return null;

			void OnCompletion(IOperation operation)
			{
				// 비동기 작업이 완료되면 이어서 명령 처리 호출.
				Continue();
			}

			var newOperation = new Operation(operation, OnCompletion);
			m_PendingQueue.Enqueue(newOperation);
			Continue();

			return newOperation;
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

					// 명령 객체가 없거나 파괴된 경우.
					if (operation == null || operation.IsDisposed)
					{
						m_PendingQueue.Dequeue();
						continue;
					}

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