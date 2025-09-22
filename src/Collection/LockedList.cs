using System;
using System.Collections.Generic;


namespace Crockhead.Core
{
	/// <summary>
	/// 스레드 안전한 리스트. (Lock 기반)
	/// </summary>
	public class LockedList<T>
	{
		/// <summary>
		/// 리스트.
		/// </summary>
		private List<T> m_Items;

		/// <summary>
		/// 락 오브젝트.
		/// </summary>
		private object m_Lock;

		/// <summary>
		/// 인덱서 프로퍼티.
		/// </summary>
		public T this[int index]
		{
			get
			{
				lock (m_Lock)
				{
					return m_Items[index];
				}
			}
			set
			{
				lock (m_Lock)
				{
					m_Items[index] = value;
				}
			}
		}

		/// <summary>
		/// 현재 개수 프로퍼티.
		/// </summary>
		public int Count
		{
			get
			{
				lock (m_Lock)
				{
					return m_Items.Count;
				}
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public LockedList()
		{
			m_Items = new List<T>();
			m_Lock = new object();
		}

		/// <summary>
		/// 항목 추가.
		/// </summary>
		public void Add(T item)
		{
			lock (m_Lock)
			{
				m_Items.Add(item);
			}
		}

		/// <summary>
		/// 여러 항목 추가.
		/// </summary>
		public void AddRange(IEnumerable<T> items)
		{
			lock (m_Lock)
			{
				m_Items.AddRange(items);
			}
		}

		/// <summary>
		/// 특정 위치에 항목 삽입.
		/// </summary>
		public void Insert(int index, T item)
		{
			lock (m_Lock)
			{
				m_Items.Insert(index, item);
			}
		}

		/// <summary>
		/// 항목 제거.
		/// </summary>
		public bool Remove(T item)
		{
			lock (m_Lock)
			{
				return m_Items.Remove(item);
			}
		}

		/// <summary>
		/// 특정 위치의 항목 제거.
		/// </summary>
		public void RemoveAt(int index)
		{
			lock (m_Lock)
			{
				m_Items.RemoveAt(index);
			}
		}

		/// <summary>
		/// 모든 항목 제거.
		/// </summary>
		public void Clear()
		{
			lock (m_Lock)
			{
				m_Items.Clear();
			}
		}

		/// <summary>
		/// 항목의 위치 반환.
		/// </summary>
		public int IndexOf(T item)
		{
			lock (m_Lock)
			{
				return m_Items.IndexOf(item);
			}
		}

		/// <summary>
		/// 항목 포함 여부 확인.
		/// </summary>
		public bool Contains(T item)
		{
			lock (m_Lock)
			{
				return m_Items.Contains(item);
			}
		}

		/// <summary>
		/// 전체 항목 스냅샷 반환.
		/// </summary>
		public T[] ToArray()
		{
			lock (m_Lock)
			{
				return m_Items.ToArray();
			}
		}

		/// <summary>
		/// 전체 항목 순회 (읽기 전용).
		/// </summary>
		public void ForEach(Action<T> action)
		{
			if (action == null)
				return;

			lock (m_Lock)
			{
				foreach (var item in m_Items)
				{
					action.Invoke(item);
				}
			}
		}

		/// <summary>
		/// 전체 항목 순회 (읽기 전용).
		/// </summary>
		public void ForEachUnlocked(Action<T> action)
		{
			if (action == null)
				return;

			var snapshot = default(T[]);
			lock (m_Lock)
			{
				snapshot = m_Items.ToArray();
			}

			foreach (var item in snapshot)
			{
				action.Invoke(item);
			}
		}
	}
}