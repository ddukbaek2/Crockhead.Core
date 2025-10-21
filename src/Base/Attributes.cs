using System;


namespace Crockhead.Core
{
	/// <summary>
	/// 특성 유틸리티.
	/// </summary>
	public static class Attributes
	{
		/// <summary>
		/// 어트리뷰트 가져오기.
		/// </summary>
		public static bool TryGetAttribute(Type instanceType, Type attributeType, out Attribute attribute)
		{
			try
			{
				return Reflections.TryGetAttribute(instanceType, attributeType, out attribute);
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 어트리뷰트 가져오기.
		/// </summary>
		public static bool TryGetAttribute<TAttribute>(Type instanceType, out TAttribute attribute) where TAttribute : Attribute
		{
			try
			{
				return Attributes.TryGetAttribute<TAttribute>(instanceType, out attribute);
			}
			catch
			{
				throw;
			}
		}
	}
}
