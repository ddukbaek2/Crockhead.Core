using System;
using System.Collections.Generic;


namespace Crockhead.Core
{
	/// <summary>
	/// 스레드 안전한 멀티맵. (Lock 기반)
	/// </summary>
	public class LockedMultiDictionary<TKey, TValue>
	{
		/// <summary>
		/// 딕셔너리.
		/// </summary>
		private readonly LockedDictionary<TKey, LockedList<TValue>> m_Dictionary;

		/// <summary>
		/// 키 갯수 프로퍼티.
		/// </summary>
		public int KeyCount => m_Dictionary.Count;

		/// <summary>
		/// 전체 값 갯수 프로퍼티. (스캔)
		/// </summary>
		public int ValueCount
		{
			get
			{
				var entries = m_Dictionary.ToArray(); // (key, bucket) 스냅샷
				var sum = 0;
				foreach (var kv in entries)
				{
					var bucket = kv.Value;
					sum += bucket.Count; // LockedList 내부 락으로 안전
				}
				return sum;
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public LockedMultiDictionary()
		{
			m_Dictionary = new LockedDictionary<TKey, LockedList<TValue>>();
		}

		/// <summary>
		/// 버킷을 얻되, 없으면 레이스 안전하게 생성하여 반환.
		/// </summary>
		private LockedList<TValue> EnsureBucket(TKey key)
		{
			// 1) 먼저 얻어보기
			if (m_Dictionary.TryGetValue(key, out var existing))
				return existing;

			// 2) 없으면 새로 시도(레이스 시 Add가 throw → 다시 얻기)
			var created = new LockedList<TValue>();
			try
			{
				m_Dictionary.Add(key, created);
				return created;
			}
			catch (ArgumentException)
			{
				// 다른 스레드가 먼저 추가한 경우
				if (m_Dictionary.TryGetValue(key, out var again))
					return again;

				// 극단적 레이스: 희박하지만 다시 시도
				return EnsureBucket(key);
			}
		}

		/// <summary>
		/// 모든 항목 제거.
		/// </summary>
		public void Clear()
		{
			var entries = m_Dictionary.ToArray();
			foreach (var kv in entries)
			{
				kv.Value.Clear();
			}

			foreach (var kv in entries)
			{
				m_Dictionary.Remove(kv.Key);
			}
		}

		/// <summary>
		/// 추가: 키가 없으면 버킷 생성 후 값 추가.
		/// </summary>
		public void Add(TKey key, TValue value)
		{
			var bucket = EnsureBucket(key);
			bucket.Add(value);
		}

		/// <summary>
		/// 여러 값 추가.
		/// </summary>
		public void AddRange(TKey key, IEnumerable<TValue> values)
		{
			if (values == null) return;
			var bucket = EnsureBucket(key);
			bucket.AddRange(values);
		}

		/// <summary>
		/// 특정 키에서 값 하나 제거.
		/// </summary>
		public bool Remove(TKey key, TValue value)
		{
			if (!m_Dictionary.TryGetValue(key, out var bucket))
				return false;

			var removed = bucket.Remove(value);
			if (!removed)
				return false;

			// 버킷이 비면 키 제거 시도(레이스 안전 재확인)
			if (bucket.Count == 0)
			{
				if (m_Dictionary.TryGetValue(key, out var current) && object.ReferenceEquals(current, bucket))
				{
					// 다시 빈지 확인(그 사이에 누군가 Add 했을 수 있으니까)
					if (bucket.Count == 0)
						m_Dictionary.Remove(key);
				}
			}
			return true;
		}

		/// <summary>
		/// 키 전체 제거.
		/// </summary>
		public int RemoveKey(TKey key)
		{
			if (!m_Dictionary.TryGetValue(key, out var bucket))
				return 0;

			var n = bucket.Count;
			bucket.Clear();
			m_Dictionary.Remove(key);
			return n;
		}

		/// <summary>
		/// 값 하나를 아무 키에서든 제거. (최초 1건)
		/// </summary>
		public bool RemoveValue(TValue value)
		{
			var entries = m_Dictionary.ToArray(); // (key, bucket) 스냅샷
			foreach (var kv in entries)
			{
				var key = kv.Key;
				var bucket = kv.Value;

				if (bucket.Remove(value))
				{
					if (bucket.Count == 0)
					{
						if (m_Dictionary.TryGetValue(key, out var current) && object.ReferenceEquals(current, bucket))
						{
							if (bucket.Count == 0)
								m_Dictionary.Remove(key);
						}
					}
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 키 존재하는지 여부 확인.
		/// </summary>
		public bool ContainsKey(TKey key)
		{
			var contains = m_Dictionary.ContainsKey(key);
			return contains;
		}

		/// <summary>
		/// 특정 키에 값 존재하는지 여부 확인.
		/// </summary>
		public bool Contains(TKey key, TValue value)
		{
			if (!m_Dictionary.TryGetValue(key, out var bucket))
				return false;

			return bucket.Contains(value);
		}

		/// <summary>
		/// 어느 키든 해당 값이 존재하는지 여부 확인.
		/// </summary>
		public bool ContainsValue(TValue value)
		{
			var entries = m_Dictionary.ToArray();

			foreach (var kv in entries)
			{
				if (kv.Value.Contains(value))
					return true;
			}

			return false;
		}

		/// <summary>
		/// 해당 값을 가진 키 하나 찾기.
		/// </summary>
		public bool TryGetKey(TValue value, out TKey key)
		{
			var entries = m_Dictionary.ToArray();
			foreach (var kv in entries)
			{
				if (kv.Value.Contains(value))
				{
					key = kv.Key;
					return true;
				}
			}
			key = default;
			return false;
		}

		/// <summary>
		/// 키가 소유한 값들을 배열로 스냅샷.
		/// </summary>
		public TValue[] ToValues(TKey key)
		{
			if (!m_Dictionary.TryGetValue(key, out var bucket))
				return Array.Empty<TValue>();

			return bucket.ToArray();
		}

		/// <summary>
		/// 모든 값을 배열로 스냅샷.
		/// </summary>
		public TValue[] ToValues()
		{
			var entries = m_Dictionary.ToArray();
			var list = new List<TValue>();
			foreach (var kv in entries)
			{
				list.AddRange(kv.Value.ToArray());
			}

			return list.ToArray();
		}

		/// <summary>
		/// 키와 값 쌍을 배열로 스냅샷.
		/// </summary>
		public KeyValuePair<TKey, TValue[]>[] ToArray()
		{
			var entries = m_Dictionary.ToArray();
			var result = new KeyValuePair<TKey, TValue[]>[entries.Length];
			for (int i = 0; i < entries.Length; i++)
			{
				result[i] = new KeyValuePair<TKey, TValue[]>(entries[i].Key, entries[i].Value.ToArray());
			}

			return result;
		}
	}
}