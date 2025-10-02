using IDotNetDisposable = System.IDisposable;


namespace Crockhead.Core
{
	/// <summary>
	/// 해제 할 수 있는 객체 인터페이스.
	/// <para>기존 IDotNetDisposable를 확장.</para>
	/// <para>using 키워드 사용 가능.</para>
	/// </summary>
	public interface IDisposable : IDotNetDisposable
	{
		/// <summary>
		/// 해제 되었는지 여부 프로퍼티.
		/// </summary>
		bool IsDisposed { get; }
	}
}