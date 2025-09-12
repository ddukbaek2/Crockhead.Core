using System;


namespace Crockhead.Core
{
	/// <summary>
	/// 제네릭 이벤트 전달자.
	/// </summary>
	public class EventDispatcher<TDelegate> : EventDispatcher where TDelegate : Delegate
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		public EventDispatcher() : base()
		{
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			base.OnDispose(explicitDisposing);
		}

		/// <summary>
		/// 등록.
		/// </summary>
		public void Register(TDelegate target)
		{
			base.Register(target);
		}

		/// <summary>
		/// 등록 해제.
		/// </summary>
		public void Unregister(TDelegate target)
		{
			base.Unregister(target);
		}

		/// <summary>
		/// 등록 여부 반환.
		/// </summary>
		public bool IsRegistered(TDelegate target)
		{
			return base.IsRegistered(target);
		}
	}
}