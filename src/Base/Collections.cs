using System.Collections.Generic;


namespace Crockhead.Core
{
	/// <summary>
	/// 컬렉션 유틸리티.
	/// </summary>
	public static class Collections
	{
		/// <summary>
		/// 리스트를 반환.
		/// </summary>
		public static List<T> ToList<T>(IEnumerable<T> enumerable)
		{
			var list = new List<T>();
			var enumerator = enumerable.GetEnumerator();
			while (enumerator.MoveNext())
			{
				list.Add(enumerator.Current);
			}

			return list;
		}

		/// <summary>
		/// 리스트를 반환.
		/// </summary>
		public static T[] ToArray<T>(IList<T> list)
		{
			var array = new T[list.Count];

			// JIT에 의해 for이 좀 더 빠르지만 미세차이이므로 로직 일관성을 위해 유지.
			//for (var i = 0; i < list.Count; ++i)
			//	array[i] = list[i];

			var enumerator = list.GetEnumerator();
			var increase = 0;
			while (enumerator.MoveNext())
			{
				array[increase] = enumerator.Current;
				++increase;
			}

			return array;
		}

		/// <summary>
		/// 갯수를 반환.
		/// </summary>
		public static int Count<T>(IEnumerable<T> enumerable)
		{
			var enumerator = enumerable.GetEnumerator();
			var increase = 0;
			while (enumerator.MoveNext())
			{
				++increase;
			}

			return increase;
		}

		/// <summary>
		/// 순서에 대한 요소를 반환.
		/// </summary>
		public static T ValueOf<T>(IEnumerable<T> enumerable, int index, T defaultValue = default(T))
		{
			var enumerator = enumerable.GetEnumerator();
			var increase = 0;
			while (enumerator.MoveNext())
			{
				if (increase == index)
					return enumerator.Current;

				++increase;
			}

			return defaultValue;
		}

		/// <summary>
		/// 요소의 순서를 반환.
		/// </summary>
		public static int IndexOf<T>(IEnumerable<T> enumerable, T value)
		{
			var enumerator = enumerable.GetEnumerator();
			var increase = 0;
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.Equals(value))
					return increase;

				++increase;
			}

			return -1;
		}

		/// <summary>
		/// 첫번째 요소 혹은 기본값을 반환.
		/// </summary>
		public static T FirstOrDefault<T>(IEnumerable<T> enumerable, T defaultValue = default(T))
		{
			var first = defaultValue;
			var enumerator = enumerable.GetEnumerator();
			if (enumerator.MoveNext())
			{
				first = enumerator.Current;
			}

			return first;
		}

		/// <summary>
		/// 마지막 요소 혹은 기본값을 반환.
		/// </summary>
		public static T LastOrDefault<T>(IEnumerable<T> enumerable, T defaultValue = default(T))
		{
			var last = defaultValue;
			var enumerator = enumerable.GetEnumerator();
			while (enumerator.MoveNext())
			{
				last = enumerator.Current;
			}

			return last;
		}
	}
}