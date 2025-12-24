using System;
using System.Threading;
using System.Threading.Tasks;


namespace Crockhead.Core
{
	/// <summary>
	/// 현재 클래스를 생성한 쓰레드의 메시지루프에서 함수를 호출하도록 동기화하는 처리기.
	/// </summary>
	public class ThreadPostDispatcher : Disposable
	{
		/// <summary>
		/// 동기화 컨텍스트.
		/// </summary>
		private SynchronizationContext m_SynchronizationContext;

		/// <summary>
		/// 동기화 컨텍스트 프로퍼티.
		/// </summary>
		public SynchronizationContext SynchronizationContext => m_SynchronizationContext;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public ThreadPostDispatcher() : base()
		{
			// 컨텍스트 캡쳐.
			m_SynchronizationContext = SynchronizationContext.Current;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			m_SynchronizationContext = null;
		}

		/// <summary>
		/// 대상 쓰레드에서 콜백 함수 호출 요청. (기본)
		/// </summary>
		protected void Post(SendOrPostCallback callback, object state)
		{
			if (callback == null)
				throw new ArgumentNullException(nameof(callback));
			if (IsDisposed)
				throw new InvalidOperationException("ThreadPostDispatcher is Disposed.");

			// 동기화 컨텍스트가 존재할 때.
			if (m_SynchronizationContext != null)
			{
				// 해당 컨텍스트를 소유한 쓰레드의 메시지 루프에서 호출되도록 요청.
				m_SynchronizationContext.Post(callback, state: state);
			}
			else
			{
				// 컨텍스트가 없다면 현재 쓰레드에서 직접 호출.
				callback.Invoke(state);
			}
		}

		/// <summary>
		/// 대상 쓰레드에서 함수 호출 요청.
		/// </summary>
		public void Post(Action action)
		{
			if (action == null)
				throw new ArgumentNullException(nameof(action));
			if (IsDisposed)
				throw new InvalidOperationException("ThreadPostDispatcher is Disposed.");

			static void Callback(object state)
			{
				var action = (Action)state;
				action.Invoke();
			}

			Post(Callback, action);
		}

		///// <summary>
		///// 대상 쓰레드에서 동기 함수를 비동기 호출 요청. (대기하는 척만 하는거라 동기 작업 실행시 멈춤 현상 생길 수 밖에 없음)
		///// </summary>
		//public Task PostAsync(Action action)
		//{
		//	if (action == null)
		//		throw new ArgumentNullException(nameof(action));
		//	if (IsDisposed)
		//		throw new InvalidOperationException("ThreadPostDispatcher is Disposed.");

		//	Post(action);
		//	return Task.CompletedTask;
		//}

		/// <summary>
		/// 대상 쓰레드에서 비동기 함수를 비동기 호출 요청.
		/// </summary>
		public Task PostAsync(Func<Task> taskFactory)
		{
			if (taskFactory == null)
				throw new ArgumentNullException(nameof(taskFactory));
			if (IsDisposed)
				throw new InvalidOperationException("ThreadPostDispatcher is Disposed.");

			var taskCompletionDispatcher = new TaskCompletionDispatcher<Object>();
			async Task Callback()
			{
				try
				{
					var task = taskFactory.Invoke();
					await taskCompletionDispatcher.RunAsync(task);
				}
				catch (Exception exception)
				{
					var task = Task.FromException(exception);
					await taskCompletionDispatcher.RunAsync(task);
				}
			}

			Post(() => _ = Callback());						
			return taskCompletionDispatcher.Task;
		}

		/// <summary>
		/// 대상 쓰레드에서 반환값이 있는 비동기 함수를 비동기 호출 요청.
		/// </summary>
		public Task<TResult> PostAsync<TResult>(Func<Task<TResult>> taskFactory)
		{
			if (taskFactory == null)
				throw new ArgumentNullException(nameof(taskFactory));
			if (IsDisposed)
				throw new InvalidOperationException("ThreadPostDispatcher is Disposed.");

			var taskCompletionDispatcher = new TaskCompletionDispatcher<TResult>();
			async Task Callback()
			{
				try
				{
					var task = taskFactory.Invoke();
					await taskCompletionDispatcher.RunAsync(task);
				}
				catch (Exception exception)
				{
					var task = Task.FromException<TResult>(exception);
					await taskCompletionDispatcher.RunAsync(task);
				}
			}

			Post(() => _ = Callback());
			return taskCompletionDispatcher.Task;
		}
	}
}