using System;
using System.Collections.Concurrent;
using System.Reflection;


namespace Crockhead.Core
{
	/// <summary>
	/// 리플렉션 기능 관련 주요 유틸리티.
	/// </summary>
	public static partial class Reflections
	{
		/// <summary>
		/// 대상 인스턴스가 대상 타입을 상속받거나 대상 타입인지 여부 반환.
		/// </summary>
		public static bool IsBaseClass(object instance, Type parentType)
		{
			if (instance == null || parentType == null)
				return false;

			var instanceType = instance.GetType();
			return IsBaseClass(instanceType, parentType);
		}

		/// <summary>
		/// 대상 인스턴스가 대상 타입을 상속받거나 대상 타입인지 여부 반환.
		/// </summary>
		public static bool IsBaseClass(Type instanceType, Type parentType)
		{
			if (instanceType == null || parentType == null)
				return false;

			return parentType.IsAssignableFrom(instanceType);
		}

		/// <summary>
		/// 인스턴스 생성.
		/// </summary>
		public static object CreateInstance(Type instanceType, params object[] arguments)
		{
			if (instanceType == null || !instanceType.IsClass)
				return null;

			try
			{
				var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
				var returnValue = Activator.CreateInstance(instanceType, bindingFlags, null, arguments, null);
				return returnValue;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 인스턴스 메소드 대리 호출.
		/// </summary>
		public static object InvokeInstanceMethod(object instance, string methodName, params object[] parameters)
		{
			if (instance == null)
				return null;

			try
			{
				var instanceType = instance.GetType();
				var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance;
				var methodInfo = instanceType.GetMethod(methodName, bindingFlags);
				if (methodInfo == null)
					return null;

				var returnValue = methodInfo.Invoke(instance, parameters);
				return returnValue;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 스태틱 메소드 대리 호출.
		/// </summary>
		public static object InvokeStaticMethod(Type instanceType, string methodName, params object[] parameters)
		{
			if (instanceType == null)
				return null;

			try
			{
				var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Static;
				var methodInfo = instanceType.GetMethod(methodName, bindingFlags);
				if (methodInfo == null)
					return null;

				var returnValue = methodInfo.Invoke(instanceType, parameters);
				return returnValue;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 특성 가져오기.
		/// </summary>
		public static bool TryGetAttribute(Type instanceType, Type attributeType, out Attribute attribute)
		{
			attribute = null;
			if (instanceType == null || attributeType == null)
				return false;

			try
			{
				attribute = instanceType.GetCustomAttribute(attributeType);
				if (attribute == null)
					return false;
				return true;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 필드 값 설정하기.
		/// </summary>
		public static bool SetFieldValue(object instance, string fieldName, object value)
		{
			if (instance == null)
				return false;
			if (string.IsNullOrWhiteSpace(fieldName))
				return false;

			try
			{
				var instanceType = instance.GetType();
				var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance;
				var fieldInfo = instanceType.GetField(fieldName, bindingFlags);
				if (fieldInfo == null)
					return false;

				fieldInfo.SetValue(instance, value);
				return true;
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// 필드 값 가져오기.
		/// </summary>
		public static object GetFieldValue(object instance, string fieldName)
		{
			if (instance == null)
				return null;
			if (string.IsNullOrWhiteSpace(fieldName))
				return null;

			try
			{
				var instanceType = instance.GetType();
				var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance;
				var fieldInfo = instanceType.GetField(fieldName, bindingFlags);
				if (fieldInfo == null)
					return null;

				var fieldValue = fieldInfo.GetValue(instance);
				return fieldValue;
			}
			catch
			{
				throw;
			}
		}
	}
}