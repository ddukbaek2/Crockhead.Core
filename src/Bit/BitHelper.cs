namespace Crockhead.Core
{
	/// <summary>
	/// 비트 유틸리티.
	/// </summary>
	public static class BitHelper
	{
		/// <summary>
		/// int의 상위 16비트를 short로 변환하여 반환.
		/// </summary>
		public static ushort HIWORD(int value)
		{
			return (ushort)((value >> 16) & 0xFFFF);
		}

		/// <summary>
		/// int의 하위 16비트를 short로 변환하여 반환.
		/// </summary>
		public static ushort LOWORD(int value)
		{
			return (ushort)(value & 0xFFFF);
		}
	}
}