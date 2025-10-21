using System;
using System.Collections;
using System.Collections.Generic;


namespace Crockhead.Core
{
	/// <summary>
	/// 집합.
	/// <para>합집합: A + B</para>
	/// <para>차집합: A - B</para>
	/// <para>교집합: A - (A - B)</para>
	/// <para>대칭차집합: (A - B) + (B - A)</para>
	/// <para>동등성 비교 할 경우 T는 IComparable 구현 필요.</para>
	/// </summary>
	public sealed class Group<T> : Disposable, IEnumerable<T> // IEquatable<Group<T>>, IReadOnlyCollection<T>
	{
		/// <summary>
		/// 집합 컬렉션.
		/// </summary>
		private HashSet<T> m_Values;

		/// <summary>
		/// 전체 요소 수 프로퍼티.
		/// </summary>
		public int Count => m_Values.Count;

		/// <summary>
		/// 집합 컬렉션 프로퍼티.
		/// </summary>
		public IEnumerable<T> Values => m_Values;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Group() : base()
		{
			m_Values = new HashSet<T>();
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Group(Group<T> group) : this()
		{
			if (group == null)
				throw new ArgumentNullException(nameof(group));

			//m_Values = new HashSet<T>(group.m_Values, group.m_Values.Comparer);
			AddRange(group);
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Group(T value) : this()
		{
			Add(value);
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Group(IEnumerable<T> values) : this()
		{
			if (values == null)
				throw new ArgumentNullException(nameof(values));

			AddRange(values);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 추가.
		/// </summary>
		public bool Add(T value)
		{
			var added = m_Values.Add(value);
			return added;
		}

		/// <summary>
		/// 추가.
		/// </summary>
		public void AddRange(IEnumerable<T> values)
		{
			if (values == null)
				throw new ArgumentNullException(nameof(values));

			foreach (var value in values)
			{
				Add(value);
			}
		}

		/// <summary>
		/// 제거.
		/// </summary>
		public bool Remove(T value)
		{
			var removed = m_Values.Remove(value);
			return removed;
		}

		/// <summary>
		/// 제거.
		/// </summary>
		public void RemoveRange(IEnumerable<T> values)
		{
			if (values == null)
				throw new ArgumentNullException(nameof(values));

			foreach (var value in values)
			{
				Remove(value);
			}
		}

		/// <summary>
		/// 포함 되어있는지 여부.
		/// </summary>
		public bool Contains(T value)
		{
			var contains = m_Values.Contains(value);
			return contains;
		}

		/// <summary>
		/// 열거자 반환.
		/// </summary>
		public IEnumerator<T> GetEnumerator()
		{
			return Values.GetEnumerator();
		}

		/// <summary>
		/// 열거자 반환.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return Values.GetEnumerator();
		}

		/// <summary>
		/// 동일 여부.
		/// </summary>
		public override bool Equals(object obj)
		{
			if (obj != null && obj is Group<T> other)
			{
				return Group<T>.Equals(this, other);
			}

			return base.Equals(obj);
		}

		/// <summary>
		/// 해시값 반환. (요소가 동일하면 해시도 동일)
		/// </summary>
		public override int GetHashCode()
		{
			return Group<T>.CreateHashCode(this);
		}

		/// <summary>
		/// 문자열 반환.
		/// </summary>
		public override string ToString()
		{
			return base.ToString();
		}

		/// <summary>
		/// 비교 연산.
		/// </summary>
		public static bool operator ==(Group<T> left, Group<T> right)
		{
			return Group<T>.Equals(left, right);
		}

		/// <summary>
		/// 비교 연산.
		/// </summary>
		public static bool operator !=(Group<T> left, Group<T> right)
		{
			return !Group<T>.Equals(left, right);
		}

		/// <summary>
		/// 더하기 연산.
		/// </summary>
		public static Group<T> operator +(Group<T> left, Group<T> right)
		{
			var group = Group<T>.Add(left, right);
			return group;
		}

		/// <summary>
		/// 더하기 연산.
		/// </summary>
		public static Group<T> operator +(Group<T> left, IEnumerable<T> right)
		{
			var group = Group<T>.Add(left, right);
			return group;
		}

		/// <summary>
		/// 빼기 연산.
		/// </summary>
		public static Group<T> operator -(Group<T> left, Group<T> right)
		{
			var group = Group<T>.Subtract(left, right);
			return group;
		}

		/// <summary>
		/// 빼기 연산.
		/// </summary>
		public static Group<T> operator -(Group<T> left, IEnumerable<T> right)
		{
			var group = Group<T>.Subtract(left, right);
			return group;
		}

		///// <summary>
		///// 형변환 연산.
		///// </summary>
		//public static implicit operator Group<T>(T value)
		//{
		//	var group = new Group<T>();
		//	group.Add(value);
		//	return group;
		//}

		///// <summary>
		///// 형변환 연산. (첫번째 요소)
		///// </summary>
		//public static implicit operator T(Group<T> group)
		//{
		//	return Collections.FirstOrDefault(group);
		//}

		///// <summary>
		///// 형변환 연산. (집합)
		///// </summary>
		//public static implicit operator T[](Group<T> group)
		//{
		//	var list = Collections.ToList(group);
		//	return Collections.ToArray(list);
		//}

		/// <summary>
		/// 그룹 생성.
		/// </summary>
		public static Group<T> Create()
		{
			return new Group<T>();
		}

		/// <summary>
		/// 그룹 생성.
		/// </summary>
		public static Group<T> Create(T value)
		{
			return new Group<T>(value);
		}

		/// <summary>
		/// 그룹 생성.
		/// </summary>
		public static Group<T> Create(IEnumerable<T> enumerable)
		{
			if (enumerable == null)
				return new Group<T>();
			return new Group<T>(enumerable);
		}

		/// <summary>
		/// 합집합.
		/// </summary>
		public static Group<T> Add(IEnumerable<T> left, IEnumerable<T> right)
		{
			var group = new Group<T>();
			if (left != null)
				group.AddRange(left);
			if (right != null)
				group.AddRange(right);
			return group;
		}

		/// <summary>
		/// 차집합.
		/// </summary>
		public static Group<T> Subtract(IEnumerable<T> left, IEnumerable<T> right)
		{
			var group = new Group<T>();
			if (left != null)
				group.AddRange(left);
			if (right != null) 
				group.RemoveRange(right);
			return group;
		}

		/// <summary>
		/// 동일 여부. (포함된 요소가 모두 동일하면 동일 판단)
		/// </summary>
		public static bool Equals(Group<T> left, Group<T> right)
		{
			if (left == null && right == null)
			{
				return true;
			}
			else if (left == null || right == null)
			{
				return false;
			}
			else
			{
				// 동일 객체.
				if (ReferenceEquals(left, right))
					return true;

				var equals = left.m_Values.SetEquals(right.m_Values);
				return equals;
			}
		}

		/// <summary>
		/// 해시 코드 생성.
		/// </summary>
		public static int CreateHashCode(Group<T> group)
		{
			if (group == null)
				return 0;

			return Group<T>.CreateHashCode(group, Comparer<T>.Default);
		}

		/// <summary>
		/// 해시 코드 생성.
		/// </summary>
		public static int CreateHashCode(Group<T> group, IComparer<T> comparer)
		{
			try
			{
				if (group == null)
					return 0;

				var hashCode = new HashCode();
				var values = new List<T>(group.m_Values);
				if (comparer == null)
				{
					// T에 IComparable 없으면 Exception.
					values.Sort();
				}
				else
				{
					values.Sort(comparer);
				}

				foreach (var value in values)
				{
					hashCode.Add(value);
				}

				return hashCode.ToHashCode();
			}
			catch
			{
				// 순서 없는 해시 코드 생성.
				return Group<T>.CreateUnorderedHashCode(group);
			}
		}

		/// <summary>
		/// 순서 없는 해시 코드 생성.
		/// </summary>
		public static int CreateUnorderedHashCode(IEnumerable<T> enumerable)
		{
			if (enumerable == null)
				return 0;

			unchecked
			{
				var xor = 0;
				var sum = 0;
				var prod = 1;
				var increase = 0;
				foreach (var value in enumerable)
				{
					var hashCode = EqualityComparer<T>.Default.GetHashCode(value);
					xor ^= hashCode;
					sum += hashCode * 16777619;
					prod = (prod * 1099511627) ^ hashCode;

					++increase;
				}

				return ((xor * 31) ^ sum) ^ (prod * 17) ^ increase;
			}
		}
	}
}