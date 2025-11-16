using System;
using System.Collections;
using System.Collections.Generic;


namespace Crockhead.Core
{
	/// <summary>
	/// 기본 제네릭 고유 식별자 생성기. (컬렉션)
	/// - IEnumerable(TResult) 인터페이스 구현체.
	/// </summary>
	public abstract class Identifiers<TIdentifier> : Disposable, IEnumerable<TIdentifier>, ICloneable
	{
		/// <summary>
		/// 할당된 고유 식별자 목록.
		/// </summary>
		private HashSet<TIdentifier> m_Values;

		/// <summary>
		/// 할당된 고유식별자 갯수 프로퍼티.
		/// </summary>
		public int Count => m_Values.Count;

		/// <summary>
		/// 할당된 고유 식별자 목록 프로퍼티.
		/// </summary>
		public IEnumerable<TIdentifier> Values => m_Values;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Identifiers() : base()
		{
			m_Values = new HashSet<TIdentifier>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			m_Values.Clear();
		}

		/// <summary>
		/// 식별자 생성.
		/// </summary>
		protected abstract TIdentifier OnCreateIdentifier();

		/// <summary>
		/// 고유 식별자 생성.
		/// - IUniqueIdentifierGenerator(TResult) 인터페이스 구현.
		/// </summary>
		public TIdentifier CreateIdentifier()
		{
			var identifier = OnCreateIdentifier();
			while (m_Values.Contains(identifier))
				identifier = OnCreateIdentifier();
			m_Values.Add(identifier);
			return identifier;
		}

		/// <summary>
		/// 비우기.
		/// </summary>
		public void Clear()
		{
			m_Values.Clear();
		}

		/// <summary>
		/// 추가.
		/// </summary>
		public bool Add(TIdentifier identifier)
		{
			var added = m_Values.Add(identifier);
			return added;
		}

		/// <summary>
		/// 추가.
		/// </summary>
		public void Add(Identifiers<TIdentifier> identifiers)
		{
			foreach (var identifier in identifiers)
				m_Values.Add(identifier);
		}

		/// <summary>
		/// 제거.
		/// </summary>
		public bool Remove(TIdentifier identifier)
		{
			var removed = m_Values.Remove(identifier);
			return removed;
		}

		/// <summary>
		/// other를 기준으로 현재 식별자 할당 상태를 최신화. (덮어쓰기)
		/// </summary>
		public void SynchronizeIdentifier(TIdentifier identifier)
		{
			m_Values.Clear();
			m_Values.Add(identifier);
		}

		/// <summary>
		/// other를 기준으로 현재 식별자 할당 상태를 최신화. (덮어쓰기)
		/// </summary>
		public void SynchronizeIdentifiers(Identifiers<TIdentifier> other)
		{
			m_Values.Clear();
			foreach (var identifier in other)
				m_Values.Add(identifier);
		}

		/// <summary>
		/// 고유 식별자 파괴. (사용 종료)
		/// </summary>
		public void DestroyIdentifier(TIdentifier identifier)
		{
			Remove(identifier);
		}

		/// <summary>
		/// 반복자 반환.
		/// <para>IEnumerable(ulong) 인터페이스 구현.</para>
		/// </summary>
		IEnumerator<TIdentifier> IEnumerable<TIdentifier>.GetEnumerator()
		{
			return m_Values.GetEnumerator();
		}

		/// <summary>
		/// 반복자 반환.
		/// <para>IEnumerable 인터페이스 구현.</para>
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_Values.GetEnumerator();
		}

		/// <summary>
		/// 복제.
		/// <para>ICloneable 인터페이스 구현.</para>
		/// </summary>
		object ICloneable.Clone()
		{
			var type = GetType();
			var identifiers = (Identifiers<TIdentifier>)Reflections.CreateInstance(type);
			identifiers.SynchronizeIdentifiers(this);
			return identifiers;
		}
	}
}