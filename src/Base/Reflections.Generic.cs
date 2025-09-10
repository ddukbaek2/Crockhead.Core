using System;


namespace Crockhead.Core
{
	/// <summary>
	/// 리플렉션 기능 관련 주요 유틸리티.
	/// </summary>
	public static partial class Reflections
	{
		/// <summary>
		/// 인스턴스 생성.
		/// </summary>
		public static T CreateInstance<T>(params object[] arguments) where T : class
		{
			try
			{
				var instanceType = typeof(T);
				var returnValue = CreateInstance(instanceType, arguments);
				if (returnValue == null)
					return default;
				return (T)returnValue;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 인스턴스 메소드 대리 호출.
		/// </summary>
		public static TReturnValue InvokeInstanceMethod<TReturnValue>(object instance, string methodName, params object[] parameters)
		{
			try
			{
				var returnValue = InvokeInstanceMethod(instance, methodName, parameters);
				if (returnValue == null)
					return default;
				return (TReturnValue)returnValue;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 어트리뷰트 가져오기.
		/// </summary>
		public static bool TryGetAttribute<TAttribute>(Type instanceType, out TAttribute attribute) where TAttribute : Attribute
		{
			attribute = default;
			try
			{
				if (!TryGetAttribute(instanceType, typeof(TAttribute), out var attrib))
					return false;
				attribute = (TAttribute)attrib;
				return true;
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}
	}
}