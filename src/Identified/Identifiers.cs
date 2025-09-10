using System.Collections;
using System.Collections.Generic;


namespace Crockhead.Core
{
	/// <summary>
	/// 제네릭 고유 식별자 생성기 + 컬렉션.
	/// - IEnumerable(T) 인터페이스 구현체.
	/// </summary>
	public abstract class Identifiers<TIdentifier> : Disposable, IEnumerable<TIdentifier>
	{
		/// <summary>
		/// 할당된 고유 식별자 목록.
		/// </summary>
		private HashSet<TIdentifier> m_Identifiers;

		/// <summary>
		/// 할당된 고유식별자 갯수.
		/// </summary>
		public int Count => m_Identifiers.Count;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Identifiers() : base()
		{
			m_Identifiers = new HashSet<TIdentifier>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			m_Identifiers.Clear();
		}

		/// <summary>
		/// 식별자 생성.
		/// </summary>
		protected abstract TIdentifier CreateIdentifier();

		/// <summary>
		/// 고유 식별자 생성.
		/// - IUniqueIdentifierGenerator(T) 인터페이스 구현.
		/// </summary>
		public TIdentifier Generate()
		{
			var identifier = CreateIdentifier();
			while (m_Identifiers.Contains(identifier))
				identifier = CreateIdentifier();
			m_Identifiers.Add(identifier);
			return identifier;
		}

		/// <summary>
		/// 비우기.
		/// </summary>
		public void Clear()
		{
			m_Identifiers.Clear();
		}

		/// <summary>
		/// other를 기준으로 현재 식별자 할당 상태를 최신화.
		/// - IUniqueIdentifierGenerator(T) 인터페이스 구현.
		/// </summary>
		public void Synchronize(Identifiers<TIdentifier> other)
		{
			m_Identifiers.Clear();
			foreach (var identifier in other)
				m_Identifiers.Add(identifier);
		}

		/// <summary>
		/// 고유 식별자 사용 종료.
		/// - IUniqueIdentifierGenerator(T) 인터페이스 구현.
		/// </summary>
		public bool Release(TIdentifier identifier)
		{
			return m_Identifiers.Remove(identifier);
		}

		/// <summary>
		/// 반복자 반환.
		/// - IEnumerable(ulong) 인터페이스 구현.
		/// </summary>
		IEnumerator<TIdentifier> IEnumerable<TIdentifier>.GetEnumerator()
		{
			return m_Identifiers.GetEnumerator();
		}

		/// <summary>
		/// 반복자 반환.
		/// - IEnumerable 인터페이스 구현.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_Identifiers.GetEnumerator();
		}
	}
}