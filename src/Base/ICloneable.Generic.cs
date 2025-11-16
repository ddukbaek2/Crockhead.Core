using System;


namespace Crockhead.Core
{
	/// <summary>
	/// 제너릭 복제 인터페이스.
	/// </summary>
	public interface ICloneable<T> : ICloneable
	{
		/// <summary>
		/// 복제.
		/// </summary>
		T Clone(bool shallowed);
	}
}