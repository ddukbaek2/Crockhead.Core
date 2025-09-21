namespace Crockhead.Core
{
	/// <summary>
	/// 통보 객체. (샘플)
	/// </summary>
	public class Notification : Disposable, INotification
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		public Notification() : base()
		{
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}
	}
}