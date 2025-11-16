using DotNetObject = System.Object;


namespace Crockhead.Core
{
	/// <summary>
	/// 기본 객체.
	/// </summary>
	public class Object : DotNetObject
	{
		/// <summary>
		/// 생성됨.
		/// </summary>
		public Object() : base()
		{
		}

		/// <summary>
		/// 파괴됨.
		/// </summary>
		~Object()
		{
		}
	}
}