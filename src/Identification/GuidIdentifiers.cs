using System;


namespace Crockhead.Core
{
	/// <summary>
	/// Guid 타입의 고유 식별자 생성기.
	/// </summary>
	public class GuidIdentifiers : Identifiers<Guid>
	{
		/// <summary>
		/// 생성.
		/// </summary>
		public GuidIdentifiers() : base()
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
		/// 식별자 생성.
		/// </summary>
		protected override Guid CreateIdentifier()
		{
			var identifier = Guid.NewGuid();
			return identifier;
		}
	}
}