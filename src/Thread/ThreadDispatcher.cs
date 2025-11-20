using System;
using System.Threading;


namespace Crockhead.Core
{
	/// <summary>
	/// 현재 클래스를 생성한 쓰레드의 메시지루프에서 함수를 실행하도록 동기화하는 처리기.
	/// </summary>
	public class ThreadDispatcher : Disposable
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
		public ThreadDispatcher() : base()
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
		/// 호출 요청.
		/// </summary>
		public void Post(Action action)
		{
			if (action == null)
				return;

			if (m_SynchronizationContext != null)
			{
				// 해당 컨텍스트를 소유한 쓰레드의 메시지루프에서 호출되도록 요청.
				m_SynchronizationContext.Post((state => action.Invoke()), null);
			}
			else
			{
				// 컨텍스트가 없으면 현재 쓰레드에서 직접 호출.
				action.Invoke();
			}
		}
	}
}