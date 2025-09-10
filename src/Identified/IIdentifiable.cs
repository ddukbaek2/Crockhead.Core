namespace Crockhead.Core
{
	/// <summary>
	/// 고유 식별자를 가질 수 있는 객체 인터페이스.
	/// </summary>
	public interface IIdentifiable<TIdentifier>
	{
		/// <summary>
		/// 객체 생성시 자동으로 생성 되어 객체의 유일성을 증명하는 고유 식별자.
		/// </summary>
		TIdentifier Identifier { get; }
	}
}