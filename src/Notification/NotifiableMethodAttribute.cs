using System;


namespace Crockhead.Core
{
	/// <summary>
	/// 통보 메서드 특성.
	/// </summary>
	public class NotifiableMethodAttribute : Attribute
	{
		/// <summary>
		/// INotification 인터페이스를 상속 받은 통지 객체의 타입.
		/// </summary>
		public Type NotificationType { set; get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public NotifiableMethodAttribute(Type notificationType) : base()
		{
			NotificationType = notificationType;

			if (NotificationType != null && !notificationType.IsAssignableFrom(typeof(INotification)))
			{
				// 오류.
			}
		}
	}
}
