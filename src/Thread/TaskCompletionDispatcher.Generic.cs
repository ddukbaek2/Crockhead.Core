using System;
using System.Threading;
using System.Threading.Tasks;


namespace Crockhead.Core
{
	/// <summary>
	/// 제네릭 태스크 완료 처리기.
	/// </summary>
	public class TaskCompletionDispatcher<TResult> : Disposable
	{
		/// <summary>
		/// 태스크 상태 처리기.
		/// </summary>
		private TaskCompletionSource<TResult> m_TaskCompletionSource;

		/// <summary>
		/// 태스크 프로퍼티.
		/// </summary>
		public Task<TResult> Task => m_TaskCompletionSource.Task;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public TaskCompletionDispatcher() : base()
		{
			m_TaskCompletionSource = new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			m_TaskCompletionSource = null;
		}

		/// <summary>
		/// 처리.
		/// </summary>
		public async Task RunAsync(Task task, TResult result = default)
		{
			if (task == null)
				throw new ArgumentNullException(nameof(task));
			if (IsDisposed)
				throw new InvalidOperationException("TaskCompletionDispatcher is Disposed.");

			var wrapedTask = TaskCompletionDispatcher<TResult>.Wrap(task, result);
			try
			{
				await wrapedTask.ConfigureAwait(continueOnCapturedContext: false);
				m_TaskCompletionSource.TrySetResult(result);
			}
			catch (OperationCanceledException)
			{
				m_TaskCompletionSource.TrySetCanceled();
			}
			catch (Exception exception)
			{
				m_TaskCompletionSource.TrySetException(exception);
			}
		}

		///// <summary>
		///// 처리.
		///// </summary>
		//public async Task RunAsync(Func<Task> taskFactory, TResult result = default)
		//{
		//	if (taskFactory == null)
		//		throw new ArgumentNullException(nameof(taskFactory));
		//	if (IsDisposed)
		//		throw new InvalidOperationException("TaskCompletionDispatcher is Disposed.");

		//	try
		//	{
		//		var task = taskFactory.Invoke();
		//		await task.ConfigureAwait(continueOnCapturedContext: false);
		//		m_TaskCompletionSource.TrySetResult(result);
		//	}
		//	catch (OperationCanceledException)
		//	{
		//		m_TaskCompletionSource.TrySetCanceled();
		//	}
		//	catch (Exception exception)
		//	{
		//		m_TaskCompletionSource.TrySetException(exception);
		//	}
		//}

		/// <summary>
		/// 처리.
		/// </summary>
		public async Task RunAsync(Task<TResult> task)
		{
			if (task == null)
				throw new ArgumentNullException(nameof(task));
			if (IsDisposed)
				throw new InvalidOperationException("TaskCompletionDispatcher is Disposed.");

			try
			{
				var result = await task.ConfigureAwait(continueOnCapturedContext: false);
				m_TaskCompletionSource.TrySetResult(result);
			}
			catch (OperationCanceledException)
			{
				m_TaskCompletionSource.TrySetCanceled();
			}
			catch (Exception exception)
			{
				m_TaskCompletionSource.TrySetException(exception);
			}
		}

		///// <summary>
		///// 처리.
		///// </summary>
		//public async Task RunAsync(Func<Task<TResult>> taskFactory)
		//{
		//	if (taskFactory == null)
		//		throw new ArgumentNullException(nameof(taskFactory));
		//	if (IsDisposed)
		//		throw new InvalidOperationException("TaskCompletionDispatcher is Disposed.");

		//	try
		//	{
		//		var task = taskFactory.Invoke();
		//		var result = await task.ConfigureAwait(continueOnCapturedContext: false);
		//		m_TaskCompletionSource.TrySetResult(result);
		//	}
		//	catch (OperationCanceledException)
		//	{
		//		m_TaskCompletionSource.TrySetCanceled();
		//	}
		//	catch (Exception exception)
		//	{
		//		m_TaskCompletionSource.TrySetException(exception);
		//	}
		//}

		/// <summary>
		/// 일반 Task를 반환값이 있는 Task<TResult>로 변환하여 반환.
		/// </summary>
		public static Task<TResult> Wrap(Task task, TResult result = default)
		{
			if (task == null)
				throw new ArgumentNullException(nameof(task));

			var taskCompletionSource = new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);
			void ContinuationAction(Task task)
			{
				if (task.IsCanceled)
				{
					taskCompletionSource.TrySetCanceled();
					return;
				}

				if (task.IsFaulted)
				{
					taskCompletionSource.TrySetException(task.Exception);
					return;
				}

				taskCompletionSource.TrySetResult(result);
			}

			task.ContinueWith(ContinuationAction, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
			return taskCompletionSource.Task;
		}
	}
}