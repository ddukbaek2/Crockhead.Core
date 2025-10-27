using System;
using System.Threading;
using IDotNetDisposable = System.IDisposable;


namespace Crockhead.Core
{
	/// <summary>
	/// 쓰레드 선점 구간 처리기.
	/// </summary>
	//public class LockedScope : Disposable
	public readonly struct LockedScope : IDotNetDisposable
	{
		/// <summary>
		/// 대상 락 오브젝트.
		/// </summary>
		private readonly object m_SynchronizationObject;

		/// <summary>
		/// 생성됨.
		/// </summary>
		//public LockedScope(object synchronizationObject, bool locked) : base()
		public LockedScope(object synchronizationObject, bool locked)
		{
			if (synchronizationObject == null)
				throw new ArgumentNullException(nameof(synchronizationObject));

			m_SynchronizationObject = synchronizationObject;

			if (locked)
			{
				Lock();
			}
		}

		///// <summary>
		///// 해제됨.
		///// </summary>
		//protected override void OnDispose(bool explicitDisposing)
		//{
		//	Unlock();
		//}

		/// <summary>
		/// 해제.
		/// </summary>
		void IDotNetDisposable.Dispose()
		{
			Unlock();
		}

		/// <summary>
		/// 락 시작.
		/// </summary>
		public void Lock()
		{
			//if (IsLocked())
			//	return;

			Monitor.Enter(m_SynchronizationObject);
		}

		/// <summary>
		/// 락 종료.
		/// </summary>
		public void Unlock()
		{
			//if (!IsLocked())
			//	return;

			Monitor.Exit(m_SynchronizationObject);
		}

		/// <summary>
		/// 락 시작 되었는지 여부.
		/// </summary>
		public bool IsLocked()
		{
			return Monitor.IsEntered(m_SynchronizationObject);
		}

		/// <summary>
		/// 락 시작.
		/// </summary>
		public static void Run(object synchronizationObject, Action action)
		{
			var lockScope = new LockedScope(synchronizationObject, false);
			try
			{
				lockScope.Lock();
				action?.Invoke();
			}
			catch
			{
				throw;
			}
			finally
			{
				lockScope.Unlock();
			}
		}
	}
}