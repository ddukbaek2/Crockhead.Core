using System;

namespace Crockhead.Core
{
	/// <summary>
	/// 결과값 있는 명령 인터페이스.
	/// </summary>
	public interface IOperation<TResult> : IOperation
	{
		/// <summary>
		/// 결과.
		/// </summary>
		TResult Result { get; }

		/// <summary>
		/// 성공. (사용금지)
		/// </summary>
		[Obsolete("Use Success(TResult result) instead. The old Success() method does not allow setting a result and is deprecated.", true)]
		new void Success();

		/// <summary>
		/// 성공.
		/// <para>기존의 Success()는 결과 설정이 불가능하므로 사용 금지.</para>
		/// </summary>
		void Success(TResult result);
	}
}