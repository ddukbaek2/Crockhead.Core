using UniqueID = System.UInt64;


namespace Crockhead.Core
{
	/// <summary>
	/// 식별 될 수 있는 객체.
	/// <para>IIdentifiable 인터페이스 구현체.</para>
	/// </summary>
	public class Identifiable : Disposable, IIdentifiable<UniqueID>
	{
		/// <summary>
		/// 기본 고유 식별자 생성기 프로퍼티.
		/// </summary>
		public static NumberIdentifiers<UniqueID> DefaultNumberIdentifiers => new NumberIdentifiers<UniqueID>();

		/// <summary>
		/// 고유 식별자 생성기.
		/// </summary>
		private NumberIdentifiers<UniqueID> m_NumberIdentifiers;

		/// <summary>
		/// 고유 식별자.
		/// </summary>
		private UniqueID m_Identifier;

		/// <summary>
		/// 고유 식별자 프로퍼티.
		/// <para>IIdentifiable 인터페이스 구현.</para>
		/// </summary>
		public UniqueID Identifier => m_Identifier;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Identifiable() : base()
		{
			m_NumberIdentifiers = DefaultNumberIdentifiers;
			m_Identifier = m_NumberIdentifiers.CreateIdentifier();
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Identifiable(UniqueID identifier) : base()
		{
			m_NumberIdentifiers = DefaultNumberIdentifiers;
			m_NumberIdentifiers.Add(identifier);
			m_Identifier = identifier;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Identifiable(NumberIdentifiers<UniqueID> identifiers) : base()
		{
			m_NumberIdentifiers = identifiers;
			m_Identifier = m_NumberIdentifiers.CreateIdentifier();
			DefaultNumberIdentifiers.SynchronizeIdentifiers(identifiers);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			DefaultNumberIdentifiers.DestroyIdentifier(m_Identifier);
		}
	}
}