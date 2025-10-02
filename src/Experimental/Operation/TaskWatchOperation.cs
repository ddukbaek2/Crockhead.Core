using Crockhead.Core;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Crockhead.Experimental
{
	/// <summary>
	/// 태스크 상태 확인 오퍼레이션.
	/// </summary>
	public class TaskWatchOperation : Operation
	{
		/// <summary>
		/// 태스크.
		/// </summary>
		private Task m_Task;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public TaskWatchOperation(Task task) : this(task, null)
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public TaskWatchOperation(Task task, Action<IOperation> completion) : base(null, completion)
		{
			if (task == null)
				throw new ArgumentNullException();

			m_Task = task;
		}

		/// <summary>
		/// 태스크 상태 확인.
		/// </summary>
		private void OnTask(Task task)
		{
			// 취소.
			if (task.IsCanceled)
			{
				base.Cancel();
			}
			// 실패.
			else if (task.IsFaulted)
			{
				var exception = task.Exception?.InnerException ?? task.Exception;
				if (exception is OperationCanceledException)
				{
					base.Cancel();
				}
				else
				{
					base.Fail(exception);
				}
			}
			// 성공.
			else
			{
				base.Success();
			}
		}

		/// <summary>
		/// 재사용.
		/// </summary>
		[Obsolete("Reset() is not supported. TaskWatchOperation cannot be reused.", true)]
		public new void Reset()
		{
			// 태스크는 일회용이므로 태스크 명령도 일회용.
			throw new NotSupportedException("TaskWatchOperation cannot be reset for reuse.");
		}

		/// <summary>
		/// 명령 설정.
		/// </summary>
		[Obsolete("SetOperation() is not supported. TaskWatchOperation always monitors the provided Task.", true)]
		public new void SetOperation(Action<IOperation> operation)
		{
			// 태스크를 대기하는 명령이 있으므로 별도 명령은 설정 불가.
			throw new InvalidOperationException("TaskWatchOperation cannot set a custom operation because it always monitors a Task.");
		}

		/// <summary>
		/// 시작됨.
		/// </summary>
		protected override void OnStarted()
		{
			base.OnStarted();

			if (m_Task == null)
			{
				// 태스크가 없다면 명령은 실패.
				var exception = new InvalidOperationException("TaskWatchOperation cannot start without a Task.");
				base.Fail(exception);
			}
			else
			{
				m_Task.ContinueWith(OnTask, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
			}
		}

		/// <summary>
		/// 성공. (호출 금지)
		/// </summary>
		[Obsolete("Success() is not supported. TaskWatchOperation determines success from the monitored Task.", true)]
		public new void Success()
		{
			// 태스크를 대기하는 명령이 있으므로 별도 상태는 설정 불가.
			throw new InvalidOperationException("TaskWatchOperation cannot set success manually. Success is determined by the Task state.");
		}

		/// <summary>
		/// 실패. (호출 금지)
		/// </summary>
		[Obsolete("Fail(Exception) is not supported. TaskWatchOperation determines failure from the monitored Task.", true)]
		public new void Fail(Exception exception)
		{
			// 태스크를 대기하는 명령이 있으므로 별도 상태는 설정 불가.
			throw new InvalidOperationException("TaskWatchOperation cannot set failure manually. Failure is determined by the Task state.");
			//base.Fail(exception);
		}

		/// <summary>
		/// 취소. (호출 금지)
		/// </summary>
		[Obsolete("Cancel() is not supported. TaskWatchOperation determines cancellation from the monitored Task.", true)]
		public new void Cancel()
		{
			// 태스크를 대기하는 명령이 있으므로 별도 상태는 설정 불가.
			throw new InvalidOperationException("TaskWatchOperation cannot set failure manually. Failure is determined by the Task state.");
		}
	}
}