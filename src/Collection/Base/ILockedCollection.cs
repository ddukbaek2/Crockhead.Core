using System.Collections.Generic;


namespace Crockhead.Core
{
	/// <summary>
	/// 쓰레드 안전한 컬렉션 인터페이스.
	/// </summary>
	public interface ILockedCollection<T> : IEnumerable<T>
	{
		/// <summary>
		/// 현재 개수 프로퍼티.
		/// </summary>
		int Count { get; }

		/// <summary>
		/// 값 목록 프로퍼티. (스냅샷)
		/// </summary>
		IEnumerable<T> Values { get; }

		/// <summary>
		/// 배열 생성.
		/// </summary>
		T[] CreateArray();

		/// <summary>
		/// 모든 항목 제거.
		/// </summary>
		void Clear();

		/// <summary>
		/// 항목 포함 여부.
		/// </summary>
		bool Contains(T value);
	}
}