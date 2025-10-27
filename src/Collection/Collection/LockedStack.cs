using System;
using System.Collections.Generic;
using System.Text;


namespace Crockhead.Core
{
	/// <summary>
	/// 쓰레드 안전한 스택.
	/// </summary>
	public class LockedStack<T> : LockedCollection<T>
	{
		/// <summary>
		/// 스택.
		/// </summary>
		private readonly Stack<T> m_Values;

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
		public LockedStack() : base()
		{
			m_Values = new Stack<T>();
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
		public void Push(T value)
		{
			using (var lockedScope = CreateLockedScope())
			{
				m_Values.Push(value);
			}
		}

		/// <summary>
		/// 제거하며 반환.
		/// </summary>
		public T Pop()
		{
			using (var lockedScope = CreateLockedScope())
			{
				var value = m_Values.Pop();
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