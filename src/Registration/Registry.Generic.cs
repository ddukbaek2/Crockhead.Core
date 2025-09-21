using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


namespace Crockhead.Core
{
	/// <summary>
	/// 등록 컨테이너.
	/// </summary>
	public class Registry<T> : Disposable, IRegistry<T>
	{
		/// <summary>
		/// 등록 대상 목록.
		/// </summary>
		private HashSet<T> m_Data;

		/// <summary>
		/// 등록된 대상 갯수 프로퍼티.
		/// </summary>
		public int Count => m_Data.Count;

		/// <summary>
		/// 반복자 프로퍼티.
		/// </summary>
		public IEnumerable<T> Values => m_Data;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Registry() : base()
		{
			m_Data = new HashSet<T>();
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			Clear();
		}

		/// <summary>
		/// 등록.
		/// </summary>
		public virtual bool Register(T target)
		{
			if (target == null)
				return false;

			if (IsRegistered(target))
				return false;

			var type = target.GetType();
			m_Data.Add(target);

			return true;
		}

		/// <summary>
		/// 등록 해제.
		/// </summary>
		public virtual bool Unregister(T target)
		{
			if (target == null)
				return false;

			if (!IsRegistered(target))
				return false;

			return m_Data.Remove(target);
		}

		/// <summary>
		/// 전체 등록 해제.
		/// </summary>
		public virtual void Clear()
		{
			m_Data.Clear();
		}

		/// <summary>
		/// 등록 되었는지 여부.
		/// </summary>
		public virtual bool IsRegistered(T target)
		{
			if (!m_Data.Contains(target))
				return false;

			return true;
		}


		/// <summary>
		/// 반복자 반환.
		/// </summary>
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return m_Data.GetEnumerator();
		}

		/// <summary>
		/// 반복자 반환.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_Data.GetEnumerator();
		}
	}
}