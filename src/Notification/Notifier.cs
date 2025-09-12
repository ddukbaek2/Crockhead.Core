using System.Collections.Generic;
using System.Reflection;


namespace Crockhead.Core
{
	/// <summary>
	/// 이벤트 통지자.
	/// </summary>
	public class Notifier : Disposable
	{
		/// <summary>
		/// 
		/// </summary>
		private Dictionary<INotifiable, MethodInfo> m_MethodInfos;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Notifier() : base()
		{
			m_MethodInfos = new Dictionary<INotifiable, MethodInfo>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 등록.
		/// </summary>
		public void Register(INotifiable target)
		{
		}

		/// <summary>
		/// 등록 해제.
		/// </summary>
		public void Unregister(INotifiable target)
		{
		}

		/// <summary>
		/// 송신.
		/// </summary>
		public void Send(object sender, INotifiable receiver)
		{
			if (!m_MethodInfos.TryGetValue(receiver, out var methodInfo))
				return;

			methodInfo.Invoke(receiver, null);
		}

		/// <summary>
		/// 통지.
		/// </summary>
		public void Notify()
		{
			
		}
	}
}