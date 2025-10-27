using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Crockhead.Core
{
	/// <summary>
	/// 스레드 안전한 해시셋. (Lock 기반)
	/// </summary>
	public class LockedSet<T> : LockedCollection<T>
	{
		/// <summary>
		/// 컬렉션.
		/// </summary>
		private readonly HashSet<T> m_Values;

		/// <summary>
		/// 현재 개수 프로퍼티.
		/// </summary>
		public override int Count
		{
			get
			{
				using (var lockedScope = CreateLockedScope())
				{
					return m_Values.Count;
				}
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public LockedSet() : base()
		{
			m_Values = new HashSet<T>();
		}

		/// <summary>
		/// 배열 생성.
		/// </summary>
		public override T[] CreateArray()
		{
			var snapshot = default(T[]);
			using (var lockedScope = CreateLockedScope())
			{
				snapshot = new T[m_Values.Count];
				m_Values.CopyTo(snapshot);
			}

			return snapshot;
		}

		/// <summary>
		/// 모든 원소 제거.
		/// </summary>
		public override void Clear()
		{
			using (var lockedScope = CreateLockedScope())
			{
				m_Values.Clear();
			}
		}

		/// <summary>
		/// 원소 포함 여부.
		/// </summary>
		public override bool Contains(T value)
		{
			using (var lockedScope = CreateLockedScope())
			{
				var contains = m_Values.Contains(value);
				return contains;
			}
		}

		/// <summary>
		/// 원소 추가.
		/// </summary>
		public bool Add(T value)
		{
			using (var lockedScope = CreateLockedScope())
			{
				var added = m_Values.Add(value);
				return added;
			}
		}

		/// <summary>
		/// 원소 제거.
		/// </summary>
		public bool Remove(T value)
		{
			using (var lockedScope = CreateLockedScope())
			{
				var removed = m_Values.Remove(value);
				return removed;
			}
		}

		/// <summary>
		/// 대상 집합의 원소를 현재 집합에 모두 추가. (합집합)
		/// </summary>
		public void UnionWith(IEnumerable<T> other)
		{
			var snapshot = CreateSnapshot(other);
			using (var lockedScope = CreateLockedScope())
			{
				m_Values.UnionWith(snapshot);
			}
		}

		/// <summary>
		/// 대상 집합과 현재 집합에 공통적으로 존재하는 원소만 보존. (교집합)
		/// </summary>
		public void IntersectWith(IEnumerable<T> other)
		{
			var snapshot = CreateSnapshot(other);
			using (var lockedScope = CreateLockedScope())
			{
				m_Values.IntersectWith(snapshot);
			}
		}

		/// <summary>
		/// 대상 집합에 있는 원소를 현재 집합에서 모두 제거. (차집합)
		/// </summary>
		public void ExceptWith(IEnumerable<T> other)
		{
			var snapshot = CreateSnapshot(other);
			using (var lockedScope = CreateLockedScope())
			{
				m_Values.ExceptWith(snapshot);
			}
		}

		/// <summary>
		/// 대상 집합과 현재 집합 중 각각 한쪽에만 존재하는 원소만 보존. (대칭차집합)
		/// </summary>
		public void SymmetricExceptWith(IEnumerable<T> other)
		{
			var snapshot = CreateSnapshot(other);
			using (var lockedScope = CreateLockedScope())
			{
				m_Values.SymmetricExceptWith(snapshot);
			}
		}

		/// <summary>
		/// 현재 집합의 모든 원소가 대상 집합에 포함되어 있는지 여부.
		/// </summary>
		public bool IsSubsetOf(IEnumerable<T> other)
		{
			var snapshot = CreateSnapshot(other);
			using (var lockedScope = CreateLockedScope())
			{
				return m_Values.IsSubsetOf(snapshot);
			}
		}

		/// <summary>
		/// 대상 집합의 모든 원소가 포함되어 있는지 여부.
		/// </summary>
		public bool IsSupersetOf(IEnumerable<T> other)
		{
			var snapshot = CreateSnapshot(other);
			using (var lockedScope = CreateLockedScope())
			{
				return m_Values.IsSupersetOf(snapshot);
			}
		}

		/// <summary>
		/// 대상 집합과 공통 원소를 하나라도 가지는지 검사.
		/// </summary>
		public bool Overlaps(IEnumerable<T> other)
		{
			var snapshot = CreateSnapshot(other);
			using (var lockedScope = CreateLockedScope())
			{
				return m_Values.Overlaps(snapshot);
			}
		}

		/// <summary>
		/// 대상 집합과 원소 목록이 동일한지 여부.
		/// </summary>
		public bool SetEquals(IEnumerable<T> other)
		{
			var snapshot = CreateSnapshot(other);
			using (var lockedScope = CreateLockedScope())
			{
				return m_Values.SetEquals(snapshot);
			}
		}
	}
}