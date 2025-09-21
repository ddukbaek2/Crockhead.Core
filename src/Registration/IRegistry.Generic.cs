using System.Collections.Generic;


namespace Crockhead.Core
{
	/// <summary>
	/// 등록 주체 인터페이스.
	/// </summary>
	public interface IRegistry<T> : IEnumerable<T>
	{
		/// <summary>
		/// 등록된 대상 갯수 프로퍼티.
		/// </summary>
		int Count { get; }

		/// <summary>
		/// 등록.
		/// </summary>
		bool Register(T target);

		/// <summary>
		/// 등록 해제.
		/// </summary>
		bool Unregister(T target);

		/// <summary>
		/// 비우기. (전체 등록 해제)
		/// </summary>
		void Clear();

		/// <summary>
		/// 등록 되었는지 여부.
		/// </summary>
		bool IsRegistered(T target);
	}
}
