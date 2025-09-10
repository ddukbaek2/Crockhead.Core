using IDotNetDisposable = System.IDisposable;


namespace Crockhead.Core
{
	/// <summary>
	/// 해제 가능한 객체 인터페이스.
	/// <para>기존 IDotNetDisposable를 확장.</para>
	/// </summary>
	public interface IDisposable : IDotNetDisposable
	{
		/// <summary>
		/// 해제 되었는지 여부 프로퍼티.
		/// </summary>
		bool IsDisposed { get; }
	}
}