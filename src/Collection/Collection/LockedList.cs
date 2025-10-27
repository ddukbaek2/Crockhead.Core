using System;
using System.Collections.Generic;


namespace Crockhead.Core
{
	/// <summary>
	/// 스레드 안전한 리스트. (Lock 기반)
	/// </summary>
	public class LockedList<T> : LockedCollection<T>
	{
		/// <summary>
		/// 리스트.
		/// </summary>
		private readonly List<T> m_Values;

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
		/// 인덱서 프로퍼티.
		/// </summary>
		public T this[int index]
		{
			get
			{
				using (var lockedScope = CreateLockedScope())
				{
					return m_Values[index];
				}
			}
			set
			{
				using (var lockedScope = CreateLockedScope())
				{
					m_Values[index] = value;
				}
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public LockedList() : base()
		{
			m_Values = new List<T>();
		}

		/// <summary>
		/// 배열 생성.
		/// </summary>
		public override T[] CreateArray()
		{
			var snapshot = default(T[]);
			using (var lockedScope = CreateLockedScope())
			{
				snapshot = m_Values.ToArray();
			}

			return snapshot;
		}

		/// <summary>
		/// 모든 항목 제거.
		/// </summary>
		public override void Clear()
		{
			using (var lockedScope = CreateLockedScope())
			{
				m_Values.Clear();
			}
		}

		/// <summary>
		/// 항목 포함 여부.
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
		/// 항목 추가.
		/// </summary>
		public void Add(T item)
		{
			using (var lockedScope = CreateLockedScope())
			{
				m_Values.Add(item);
			}
		}

		/// <summary>
		/// 여러 항목 추가.
		/// </summary>
		public void AddRange(IEnumerable<T> items)
		{
			using (var lockedScope = CreateLockedScope())
			{
				m_Values.AddRange(items);
			}
		}

		/// <summary>
		/// 특정 위치에 항목 삽입.
		/// </summary>
		public void Insert(int index, T item)
		{
			using (var lockedScope = CreateLockedScope())
			{
				m_Values.Insert(index, item);
			}
		}

		/// <summary>
		/// 항목 제거.
		/// </summary>
		public bool Remove(T item)
		{
			using (var lockedScope = CreateLockedScope())
			{
				return m_Values.Remove(item);
			}
		}

		/// <summary>
		/// 특정 위치의 항목 제거.
		/// </summary>
		public void RemoveAt(int index)
		{
			using (var lockedScope = CreateLockedScope())
			{
				m_Values.RemoveAt(index);
			}
		}

		/// <summary>
		/// 항목의 위치 반환.
		/// </summary>
		public int IndexOf(T item)
		{
			using (var lockedScope = CreateLockedScope())
			{
				return m_Values.IndexOf(item);
			}
		}

		/// <summary>
		/// 전체 항목 순회 (읽기 전용).
		/// </summary>
		public void ForEach(Action<T> action)
		{
			if (action == null)
				return;

			using (var lockedScope = CreateLockedScope())
			{
				foreach (var item in m_Values)
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
			using (var lockedScope = CreateLockedScope())
			{
				snapshot = m_Values.ToArray();
			}

			foreach (var item in snapshot)
			{
				action.Invoke(item);
			}
		}
	}
}