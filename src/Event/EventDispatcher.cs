using System;


namespace Crockhead.Core
{
	/// <summary>
	/// 이벤트 전달자.
	/// </summary>
	public class EventDispatcher : Disposable
	{
		/// <summary>
		/// 메서드 목록.
		/// </summary>
		private Delegate m_Delegate;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public EventDispatcher() : base()
		{
			m_Delegate = null;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			Clear();
		}

		/// <summary>
		/// 등록.
		/// </summary>
		public void Register(Delegate target)
		{
			if (target == null)
				return;

			m_Delegate = Delegate.Combine(m_Delegate, target);
		}

		/// <summary>
		/// 등록 해제.
		/// </summary>
		public void Unregister(Delegate target)
		{
			if (m_Delegate == null || target == null)
				return;

			m_Delegate = Delegate.Remove(m_Delegate, target);
		}

		/// <summary>
		/// 전체 비우기.
		/// </summary>
		public void Clear()
		{
			m_Delegate = null;
		}

		/// <summary>
		/// 등록 여부 반환.
		/// </summary>
		public bool IsRegistered(Delegate target)
		{
			if (m_Delegate == null || target == null)
				return false;

			var invocationList = m_Delegate.GetInvocationList();
			foreach (var invocation in invocationList)
			{
				if (invocation == target)
					return true;
			}

			return false;
		}

		/// <summary>
		/// 등록된 모든 개체에게 이벤트 전달.
		/// </summary>
		public void Dispatch(params object[] arguments)
		{
			// TOCTOU 회피.
			var snapshot = m_Delegate;
			if (snapshot == null)
				return;

			var invocationList = snapshot.GetInvocationList();
			foreach (var invocation in invocationList)
			{
				try
				{
					invocation.Method.Invoke(invocation.Target, arguments);
				}
				catch// (Exception exception)
				{
					//throw;
					//throw exception;
				}
			}
		}
	}
}