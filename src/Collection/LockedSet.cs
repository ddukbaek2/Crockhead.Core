using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Crockhead.Core
{
	/// <summary>
	/// 스레드 안전한 해시셋. (Lock 기반)
	/// </summary>
	public class LockedSet<T> : IEnumerable<T>
	{
		/// <summary>
		/// 락 오브젝트.
		/// </summary>
		private readonly object m_Lock;

		/// <summary>
		/// 컬렉션.
		/// </summary>
		private readonly HashSet<T> m_Values;

		/// <summary>
		/// 현재 개수 프로퍼티.
		/// </summary>
		public int Count
		{
			get
			{
				lock (m_Lock)
				{
					return m_Values.Count;
				}
			}
		}

		/// <summary>
		/// 값 목록 프로퍼티. (스냅샷)
		/// </summary>
		public IEnumerable<T> Values
		{
			get
			{
				var snapshot = ToArray();
				return snapshot;
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public LockedSet()
		{
			m_Lock = new object();
			m_Values = new HashSet<T>();
		}

		/// <summary>
		/// 모든 원소 제거.
		/// </summary>
		public void Clear()
		{
			lock (m_Lock)
			{
				m_Values.Clear();
			}
		}

		/// <summary>
		/// 원소 추가.
		/// </summary>
		public bool Add(T value)
		{
			lock (m_Lock)
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
			lock (m_Lock)
			{
				var removed = m_Values.Remove(value);
				return removed;
			}
		}

		/// <summary>
		/// 원소 포함 여부.
		/// </summary>
		public bool Contains(T value)
		{
			lock (m_Lock)
			{
				var contains = m_Values.Contains(value);
				return contains;
			}
		}

		/// <summary>
		/// 대상 집합의 원소를 현재 집합에 모두 추가. (합집합)
		/// </summary>
		public void UnionWith(IEnumerable<T> other)
		{
			lock (m_Lock)
			{
				m_Values.UnionWith(other);
			}
		}

		/// <summary>
		/// 대상 집합과 현재 집합에 공통적으로 존재하는 원소만 보존. (교집합)
		/// </summary>
		public void IntersectWith(IEnumerable<T> other)
		{
			lock (m_Lock)
			{
				m_Values.IntersectWith(other);
			}
		}

		/// <summary>
		/// 대상 집합에 있는 원소를 현재 집합에서 모두 제거. (차집합)
		/// </summary>
		public void ExceptWith(IEnumerable<T> other)
		{
			lock (m_Lock)
			{
				m_Values.ExceptWith(other);
			}
		}

		/// <summary>
		/// 대상 집합과 현재 집합 중 각각 한쪽에만 존재하는 원소만 보존. (대칭차집합)
		/// </summary>
		public void SymmetricExceptWith(IEnumerable<T> other)
		{
			lock (m_Lock)
			{
				m_Values.SymmetricExceptWith(other);
			}
		}

		/// <summary>
		/// 현재 집합의 모든 원소가 대상 집합에 포함되어 있는지 여부.
		/// </summary>
		public bool IsSubsetOf(IEnumerable<T> other)
		{
			lock (m_Lock)
			{
				return m_Values.IsSubsetOf(other);
			}
		}

		/// <summary>
		/// 대상 집합의 모든 원소가 포함되어 있는지 여부.
		/// </summary>
		public bool IsSupersetOf(IEnumerable<T> other)
		{
			lock (m_Lock)
			{
				return m_Values.IsSupersetOf(other);
			}
		}

		/// <summary>
		/// 대상 집합과 공통 원소를 하나라도 가지는지 검사.
		/// </summary>
		public bool Overlaps(IEnumerable<T> other)
		{
			lock (m_Lock)
			{
				return m_Values.Overlaps(other);
			}
		}

		/// <summary>
		/// 대상 집합과 원소 목록이 동일한지 여부.
		/// </summary>
		public bool SetEquals(IEnumerable<T> other)
		{
			lock (m_Lock)
			{
				return m_Values.SetEquals(other);
			}
		}

		/// <summary>
		/// 전체 항목 스냅샷 반환.
		/// </summary>
		public T[] ToArray()
		{
			var snapshot = default(T[]);
			lock (m_Lock)
			{
				snapshot = new T[m_Values.Count];
				m_Values.CopyTo(snapshot);
			}

			return snapshot;
		}

		/// <summary>
		/// 반복자 반환.
		/// </summary>
		public IEnumerator<T> GetEnumerator()
		{
			var snapshot = ToArray();
			for (var i = 0; i < snapshot.Length; ++i)
			{
				yield return snapshot[i];
			}
		}

		/// <summary>
		/// 반복자 반환.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}