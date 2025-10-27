using System;
using System.Collections.Generic;
using System.Text;


namespace Crockhead.Core
{
	/// <summary>
	/// 쓰레드 안전한 큐.
	/// </summary>
	public class LockedQueue<T> : LockedCollection<T>
	{
		/// <summary>
		/// 큐.
		/// </summary>
		private readonly Queue<T> m_Values;

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
		public LockedQueue() : base()
		{
			m_Values = new Queue<T>();
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
		/// 추가.
		/// </summary>
		public void Enqueue(T value)
		{
			using (var lockedScope = CreateLockedScope())
			{
				m_Values.Enqueue(value);
			}
		}

		/// <summary>
		/// 제거하며 반환.
		/// </summary>
		public T Dequeue()
		{
			using (var lockedScope = CreateLockedScope())
			{
				var value = m_Values.Dequeue();
				return value;
			}
		}

		/// <summary>
		/// 제거하지 않고 반환.
		/// </summary>
		public T Peek()
		{
			using (var lockedScope = CreateLockedScope())
			{
				var value = m_Values.Peek();
				return value;
			}
		}
	}
}