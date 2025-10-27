using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Crockhead.Core
{
	/// <summary>
	/// 쓰레드 안전한 컬렉션.
	/// </summary>
	public abstract class LockedCollection<T> : ILockedCollection<T>
	{
		/// <summary>
		/// 락 오브젝트.
		/// </summary>
		private readonly object m_SynchronizationObject;

		/// <summary>
		/// 현재 개수 프로퍼티.
		/// </summary>
		public abstract int Count { get; }
	
		/// <summary>
		/// 값 목록 프로퍼티. (스냅샷)
		/// </summary>
		public IEnumerable<T> Values
		{
			get
			{
				var snapshot = CreateArray();
				return snapshot;
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public LockedCollection()
		{
			m_SynchronizationObject = new object();
		}

		/// <summary>
		/// 락 생성.
		/// </summary>
		protected LockedScope CreateLockedScope(bool locked = true)
		{
			var lockScope = new LockedScope(m_SynchronizationObject, locked);
			return lockScope;
		}

		/// <summary>
		/// 배열 생성.
		/// </summary>
		public abstract T[] CreateArray();

		/// <summary>
		/// 모든 항목 제거.
		/// </summary>
		public abstract void Clear();

		/// <summary>
		/// 포함 여부.
		/// </summary>
		public abstract bool Contains(T value);

		/// <summary>
		/// 반복자 반환.
		/// </summary>
		public IEnumerator<T> GetEnumerator()
		{
			var snapshot = CreateSnapshot(this);
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

		/// <summary>
		/// 반복자의 스냅샷 생성.
		/// </summary>
		public static T[] CreateSnapshot(IEnumerable<T> enumerable)
		{
			if (enumerable == null)
				throw new ArgumentNullException(nameof(enumerable));

			if (enumerable is LockedCollection<T> collection)
			{
				return collection.CreateArray();
			}
			else if (enumerable is T[] array)
			{
				return (T[])array.Clone();
			}
			else
			{
				// System.Linq.Enumerable.ToArray()
				return enumerable.ToArray();
			}
		}
	}
}