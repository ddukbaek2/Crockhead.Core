using System;
using System.Collections.Generic;
using System.Linq;


namespace Crockhead.Unity
{
	/// <summary>
	/// 반복체.
	/// </summary>
	public class Iteratable<T>
	{
		/// <summary>
		/// 반복자.
		/// </summary>
		public readonly struct Iterator
		{
			/// <summary>
			/// 빈 이터레이터.
			/// </summary>
			public static readonly Iterator Null = new Iterator(null, 0);

			/// <summary>
			/// 반복체.
			/// </summary>
			public readonly Iteratable<T> Iteratable { get; } // 8byte.

			/// <summary>
			/// 인덱스.
			/// </summary>
			public readonly int Index { get; } // 4byte.

			/// <summary>
			/// 생성됨.
			/// </summary>
			public Iterator(Iteratable<T> iteratable, int index)
			{
				Iteratable = iteratable;
				Index = index;
			}

			/// <summary>
			/// 요소의 값 참조 반환.
			/// </summary>
			public ref T Value()
			{
				if (Iteratable == null)
					throw new ArgumentNullException(nameof(Iteratable));

				return ref Iteratable.GetValue(Index);
			}

			/// <summary>
			/// 처음 요소.
			/// </summary>
			public ref Iterator Begin()
			{
				if (Iteratable == null)
					throw new InvalidOperationException(nameof(Iteratable));

				return ref Iteratable.Begin();
			}

			/// <summary>
			/// 마지막 요소.
			/// </summary>
			public ref Iterator End()
			{
				if (Iteratable == null)
					throw new InvalidOperationException(nameof(Iteratable));

				return ref Iteratable.End();
			}

			/// <summary>
			/// 이전 요소.
			/// </summary>
			public ref Iterator Previous()
			{
				if (Iteratable == null)
					throw new InvalidOperationException(nameof(Iteratable));

				return ref Iteratable.Previous(Index);
			}

			/// <summary>
			/// 다음 요소.
			/// </summary>
			public ref Iterator Next()
			{
				if (Iteratable == null)
					throw new InvalidOperationException(nameof(Iteratable));

				return ref Iteratable.Next(Index);
			}

			/// <summary>
			/// 처음 요소 여부.
			/// </summary>
			public bool IsBegin()
			{
				if (Iteratable == null)
					throw new InvalidOperationException(nameof(Iteratable));

				ref var current = ref Iteratable.GetIterator(Index);
				ref var begin = ref Iteratable.Begin();
				var equals = Equals(current, begin);
				return equals;
			}

			/// <summary>
			/// 마지막 요소 여부.
			/// </summary>
			public bool IsEnd()
			{
				if (Iteratable == null)
					throw new InvalidOperationException(nameof(Iteratable));

				ref var current = ref Iteratable.GetIterator(Index);
				ref var end = ref Iteratable.End();
				var equals = Equals(current, end);
				return equals;

			}

			/// <summary>
			/// 동등성 비교.
			/// </summary>
			public static bool Equals(ref Iterator left, ref Iterator right)
			{
				if (left.Iteratable != right.Iteratable)
					return false;
				if (left.Index != right.Index)
					return false;

				return true;
			}

			/// <summary>
			/// 이전 요소 이동.
			/// </summary>
			public static Iterator operator --(Iterator iterator)
			{
				return iterator.Previous();
			}

			/// <summary>
			/// 다음 요소 이동.
			/// </summary>
			public static Iterator operator ++(Iterator iterator)
			{
				return iterator.Next();
			}
		}


		/// <summary>
		/// 값 목록.
		/// </summary>
		private T[] m_Values;

		/// <summary>
		/// 이터레이터 목록.
		/// </summary>
		private Iterator[] m_Iterators;

		/// <summary>
		/// 갯수.
		/// </summary>
		private int m_Count;

		/// <summary>
		/// 갯수 프로퍼티.
		/// </summary>
		public int Count => m_Count;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Iteratable(IEnumerable<T> values)
		{
			if (values == null)
				throw new ArgumentNullException(nameof(values));

			m_Values = values.ToArray();
			m_Count = m_Values.Length;
			m_Iterators = new Iterator[m_Count];
			for (var index = 0; index < m_Count; ++index)
			{
				m_Iterators[index] = new Iterator(this, index);
			}
		}

		/// <summary>
		/// 값 반환.
		/// </summary>
		public ref T GetValue(int index)
		{
			if (index < 0 || index >= m_Count)
				throw new InvalidOperationException();

			return ref m_Values[index];
		}

		/// <summary>
		/// 반복자 반환.
		/// </summary>
		public ref Iterator GetIterator(int index)
		{
			if (index < 0 || index >= m_Count)
				throw new InvalidOperationException();

			return ref m_Iterators[index];
		}

		/// <summary>
		/// 처음 요소.
		/// </summary>
		public ref Iterator Begin()
		{
			return ref GetIterator(0);
		}

		/// <summary>
		/// 마지막 요소.
		/// </summary>
		public ref Iterator End()
		{
			return ref GetIterator(m_Count - 1);
		}

		/// <summary>
		/// 이전 요소.
		/// </summary>
		internal ref Iterator Previous(int index)
		{
			return ref GetIterator(index - 1);
		}

		/// <summary>
		/// 다음 요소.
		/// </summary>
		internal ref Iterator Next(int index)
		{
			return ref GetIterator(index + 1);

		}

		/// <summary>
		/// 유효한 이터레이터인지 여부.
		/// </summary>
		public bool Validate(Iterator iterator)
		{
			if (iterator.Iteratable == null)
				return false;
			if (iterator.Index < 0 || iterator.Index >= m_Count)
				return false;

			return true;
		}

		///// <summary>
		///// 사용 샘플.
		///// </summary>
		//private static void DoExample(Iteratable<TResult> iteratable)
		//{
		//	ref var it = ref iterable.Begin();
		//	ref var end = ref iterable.End();

		// 이터레이터가 복사되어 약간의 손실은 있지만 간편한 버전.
		//for (var it = iteratable.Begin(); it != iteratable.End(); ++it)
		//	ref var v = ref it.GetValue();

		//	while (true)
		//	{
		//		// 계속 다음 요소로 접근.
		//		it = ref iterable.Next(it.Index)

		//		Debug.Log(it.GetValue());

		//		// 마지막 요소면 탈출.
		//		if (Iterator.Equals(ref it, ref end))
		//			break;
		//	}
		//}
	}
}