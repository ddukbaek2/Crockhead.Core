using System;
using System.Threading;
using System.Threading.Tasks;


namespace Crockhead.Core
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
		private void OnWatch(Task task)
		{
			if (task.IsCanceled)
			{
				base.Cancel();
			}
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
			else
			{
				base.Success();
			}
		}

		/// <summary>
		/// 재사용.
		/// </summary>
		public override void Reset()
		{
			// 태스크는 일회용이므로 태스크 명령도 일회용.
			throw new NotSupportedException("TaskWatchOperation cannot be reset for reuse.");
			//base.Reset();
		}

		/// <summary>
		/// 명령 설정.
		/// </summary>
		public override void SetOperation(Action<IOperation> operation)
		{
			// 태스크를 대기하는 명령이 있으므로 별도 명령은 설정 불가.
			throw new InvalidOperationException();
			//base.SetOperation(operation);
		}

		/// <summary>
		/// 시작.
		/// </summary>
		public override void Start()
		{
			if (IsStarted)
				return;

			base.Start();
			if (m_Task == null)
			{
				// 태스크가 없다면 명령은 실패.
				base.Fail(new InvalidOperationException());
			}
			else
			{
				m_Task.ContinueWith(OnWatch, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
			}
		}

		/// <summary>
		/// 성공.
		/// </summary>
		public override void Success()
		{
			// 태스크를 대기하는 명령이 있으므로 별도 상태는 설정 불가.
			throw new InvalidOperationException();
			//base.Success();
		}

		/// <summary>
		/// 실패.
		/// </summary>
		public override void Fail(Exception exception)
		{
			// 태스크를 대기하는 명령이 있으므로 별도 상태는 설정 불가.
			throw new InvalidOperationException();
			//base.Fail(exception);
		}

		/// <summary>
		/// 취소.
		/// </summary>
		public override void Cancel()
		{
			// 태스크를 대기하는 명령이 있으므로 별도 상태는 설정 불가.
			throw new InvalidOperationException();
			//base.Cancel();
		}
	}
}