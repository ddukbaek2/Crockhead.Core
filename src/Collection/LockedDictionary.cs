using System;
using System.Collections.Generic;


namespace Crockhead.Core
{
	/// <summary>
	/// 쓰레드 안전한 딕셔너리. (Lock 기반)
	/// </summary>
	public class LockedDictionary<TKey, TValue>
	{
		/// <summary>
		/// 락 오브젝트.
		/// </summary>
		private readonly object m_Lock;

		/// <summary>
		/// 딕셔너리.
		/// </summary>
		private readonly Dictionary<TKey, TValue> m_Items;

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
		/// 인덱서 프로퍼티.
		/// </summary>
		public TValue this[TKey key]
		{
			get
			{
				lock (m_Lock)
				{
					return m_Items[key];
				}
			}
			set
			{
				lock (m_Lock)
				{
					m_Items[key] = value;
				}
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public LockedDictionary(IEqualityComparer<TKey> equalityComparer = null)
		{
			if (equalityComparer == null)
				equalityComparer = EqualityComparer<TKey>.Default;

			m_Lock = new object();
			m_Items = new Dictionary<TKey, TValue>(equalityComparer);
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
		/// 항목 추가.
		/// </summary>
		public bool TryAdd(TKey key, TValue value)
		{
			lock (m_Lock)
			{
				if (m_Items.ContainsKey(key))
					return false;

				m_Items.Add(key, value);
				return true;
			}
		}

		/// <summary>
		/// 항목 추가.
		/// </summary>
		public void AddOrSet(TKey key, TValue value)
		{
			lock (m_Lock)
			{
				m_Items[key] = value;
			}
		}

		/// <summary>
		/// 항목 추가.
		/// </summary>
		public TValue GetOrAdd(TKey key, Func<TKey, TValue> factory)
		{
			lock (m_Lock)
			{
				if (m_Items.TryGetValue(key, out var v))
					return v;

				v = factory(key);
				m_Items.Add(key, v);
				return v;
			}
		}

		/// <summary>
		/// 항목 제거.
		/// </summary>
		public bool TryRemove(TKey key, out TValue value)
		{
			lock (m_Lock)
			{
				if (m_Items.TryGetValue(key, out value))
				{
					m_Items.Remove(key);
					return true;
				}

				return false;
			}
		}

		/// <summary>
		/// 항목 추가.
		/// </summary>
		public void Add(TKey key, TValue value)
		{
			lock (m_Lock)
			{
				m_Items.Add(key, value);
			}
		}

		/// <summary>
		/// 항목 제거.
		/// </summary>
		public bool Remove(TKey key)
		{
			lock (m_Lock)
			{
				var removed = m_Items.Remove(key);
				return removed;
			}
		}

		/// <summary>
		/// 항목 포함 여부 확인.
		/// </summary>
		public bool ContainsKey(TKey key)
		{
			lock (m_Lock)
			{
				var contains = m_Items.ContainsKey(key);
				return contains;
			}
		}

		/// <summary>
		/// 항목 포함 여부 확인.
		/// </summary>
		public bool ContainsValue(TValue value)
		{
			var eqaulityComparer = EqualityComparer<TValue>.Default;
			lock (m_Lock)
			{
				foreach (var item in m_Items)
				{
					if (eqaulityComparer.Equals(item.Value, value))
					{
						return true;
					}
				}

				return false;
			}
		}

		/// <summary>
		/// 키 반환.
		/// </summary>
		public bool TryGetKey(TValue value, out TKey key)
		{
			var eqaulityComparer = EqualityComparer<TValue>.Default;
			lock (m_Lock)
			{
				foreach (var item in m_Items)
				{
					if (eqaulityComparer.Equals(item.Value, value))
					{
						key = item.Key;
						return true;
					}
				}

				key = default(TKey);
				return false;
			}
		}

		/// <summary>
		/// 항목 반환.
		/// </summary>
		public bool TryGetValue(TKey key, out TValue value)
		{
			lock (m_Lock)
			{
				var contains = m_Items.TryGetValue(key, out value);
				return contains;
			}
		}

		/// <summary>
		/// 전체 항목 스냅샷 반환.
		/// </summary>
		public TValue[] ToValues()
		{
			lock (m_Lock)
			{
				var snapshot = new TValue[m_Items.Count];
				var index = 0;
				foreach (var value in m_Items.Values)
				{
					snapshot[index++] = value;
				}
				return snapshot;
			}
		}

		/// <summary>
		/// 전체 항목 스냅샷 반환.
		/// </summary>
		public KeyValuePair<TKey, TValue>[] ToArray()
		{
			lock (m_Lock)
			{
				var snapshot = new KeyValuePair<TKey, TValue>[m_Items.Count];
				var index = 0;
				foreach (var item in m_Items)
				{
					snapshot[index++] = new KeyValuePair<TKey, TValue>(item.Key, item.Value);
				}
				return snapshot;
			}
		}
	}
}