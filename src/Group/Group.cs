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
	/// </summary>
	public sealed class Group<TElement> : Disposable, IEnumerable<TElement>
	{
		/// <summary>
		/// 집합 컬렉션.
		/// </summary>
		//private HashSet<TElement> m_Values;
		private List<TElement> m_Values;

		///// <summary>
		///// 해시코드.
		///// </summary>
		//private HashCode m_HashCode;

		/// <summary>
		/// 전체 요소 수 프로퍼티.
		/// </summary>
		public int Count => m_Values.Count;

		/// <summary>
		/// 집합 컬렉션 프로퍼티.
		/// </summary>
		public IEnumerable<TElement> Values => m_Values;

		/// <summary>
		/// 인덱서 프로퍼티.
		/// </summary>
		public TElement this[int index]
		{
			get
			{
				//var value = Collections.ValueOf(m_Values, index);
				//return value;
				return m_Values[index];
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Group() : base()
		{
			//m_Values = new HashSet<TElement>();
			m_Values = new List<TElement>();
			//m_HashCode = new HashCode();
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Group(TElement value) : this()
		{
			Add(value);
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Group(IEnumerable<TElement> values) : this()
		{
			AddRange(values);
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Group(Group<TElement> group) : this()
		{
			AddRange(group);
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
		public bool Add(TElement value)
		{
			var contains = Contains(value);
			if (contains)
				return false;
			
			m_Values.Add(value);
			return true;
		}

		/// <summary>
		/// 추가.
		/// </summary>
		public void AddRange(IEnumerable<TElement> values)
		{
			foreach (var value in values)
			{
				Add(value);
			}
		}

		/// <summary>
		/// 제거.
		/// </summary>
		public bool Remove(TElement value)
		{
			var removed = m_Values.Remove(value);
			return removed;
		}

		/// <summary>
		/// 제거.
		/// </summary>
		public void RemoveRange(IEnumerable<TElement> values)
		{
			foreach (var value in values)
			{
				Remove(value);
			}
		}

		/// <summary>
		/// 포함 되어있는지 여부.
		/// </summary>
		public bool Contains(TElement value)
		{
			var contains = m_Values.Contains(value);
			return contains;
		}

		/// <summary>
		/// 열거자 반환.
		/// </summary>
		public IEnumerator<TElement> GetEnumerator()
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
		/// 동일 여부. (포함된 요소가 모두 동일하면 동일 판단)
		/// </summary>
		public bool Equals(Group<TElement> other)
		{
			var count = m_Values.Count;

			// 요소의 수가 다르다면 동일하지 않음 판단.
			if (count != other.m_Values.Count)
				return false;

			// other에 포함되지 않은 자신의 요소 발견 시 동일하지 않음 판단.
			for (var i = 0; i < count; ++i)
			{
				var value = m_Values[i];
				if (other.Contains(value))
					continue;

				return false;
			}

			// 자신에게 포함되지 않은 other의 요소 발견 시 동일하지 않음 판단.
			for (var i = 0; i < count; ++i)
			{
				var value = other.m_Values[i];
				if (m_Values.Contains(value))
					continue;

				return false;
			}

			return true;
		}

		/// <summary>
		/// 해시코드 변환.
		/// </summary>
		public int ToHashCode()
		{
			var hashCode = new HashCode();
			foreach (var value in m_Values)
			{
				hashCode.Add(value);
			}

			return hashCode.ToHashCode();
		}

		/// <summary>
		/// 동일 여부.
		/// </summary>
		public override bool Equals(object obj)
		{
			if (obj is Group<TElement> other)
			{
				return Equals(other);
			}

			return base.Equals(obj);
		}

		/// <summary>
		/// 해시값 반환. (요소가 동일하면 해시도 동일)
		/// </summary>
		public override int GetHashCode()
		{
			return ToHashCode();
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
		public static bool operator ==(Group<TElement> left, Group<TElement> right)
		{
			return left.Equals(right);
		}

		/// <summary>
		/// 비교 연산.
		/// </summary>
		public static bool operator !=(Group<TElement> left, Group<TElement> right)
		{
			return !left.Equals(right);
		}

		/// <summary>
		/// 더하기 연산.
		/// </summary>
		public static Group<TElement> operator +(Group<TElement> left, Group<TElement> right)
		{
			var group = new Group<TElement>();
			group.AddRange(left);
			group.AddRange(right);
			return group;
		}

		/// <summary>
		/// 빼기 연산.
		/// </summary>
		public static Group<TElement> operator -(Group<TElement> left, Group<TElement> right)
		{
			var group = new Group<TElement>();
			group.AddRange(left);
			group.RemoveRange(right);
			return group;
		}

		/// <summary>
		/// 형변환 연산.
		/// </summary>
		public static implicit operator Group<TElement>(TElement value)
		{
			var group = new Group<TElement>();
			group.Add(value);
			return group;
		}

		/// <summary>
		/// 형변환 연산. (첫번째 요소)
		/// </summary>
		public static implicit operator TElement(Group<TElement> group)
		{
			return Collections.FirstOrDefault(group);
		}

		/// <summary>
		/// 형변환 연산. (집합)
		/// </summary>
		public static implicit operator TElement[](Group<TElement> group)
		{
			var list = Collections.ToList(group);
			return Collections.ToArray(list);
		}
	}
}