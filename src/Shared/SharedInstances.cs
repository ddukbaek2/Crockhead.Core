using System;
using System.Collections.Generic;


namespace Crockhead.Core
{
	/// <summary>
	/// 공유 인스턴스 유틸리티.
	/// <para>제너릭 캐싱이라 박싱,언박싱 없고 접근비용 O(1) 및 필드 접근 수준의 히트캐시.</para>
	/// </summary>
	public static class SharedInstances
	{
		/// <summary>
		/// 값을 보관하는 객체.
		/// </summary>
		private static class Container<T>
		{
			/// <summary>
			/// 값 존재 여부.
			/// </summary>
			private static bool s_IsSet = false;

			/// <summary>
			/// 값.
			/// </summary>
			private static T s_Value = default;

			/// <summary>
			/// 값 채우기.
			/// </summary>
			public static void Set(T value)
			{
				s_Value = value;
				s_IsSet = true;
			}

			/// <summary>
			/// 값 비우기.
			/// </summary>
			public static void Unset()
			{
				s_Value = default;
				s_IsSet = false;
			}

			/// <summary>
			/// 값 반환.
			/// </summary>
			public static T Get()
			{
				return s_Value;
			}

			/// <summary>
			/// 값 존재 여부.
			/// </summary>
			public static bool IsSet()
			{
				return s_IsSet;
			}
		}


		/// <summary>
		/// 값 비우기 액션 목록.
		/// </summary>
		private static readonly Dictionary<Type, Action> s_unsetActions = new Dictionary<Type, Action>();

		/// <summary>
		/// 레퍼런스 복제용.
		/// </summary>
		private static readonly List<Action> s_TemporaryActions = new List<Action>();

		/// <summary>
		/// 등록 해제.
		/// </summary>
		private static void Unregister(Type key)
		{
			s_unsetActions.Remove(key);
		}

		/// <summary>
		/// 전체 값 비우기.
		/// </summary>
		public static void Clear()
		{
			s_TemporaryActions.Clear();
			s_TemporaryActions.AddRange(s_unsetActions.Values);
			foreach (var action in s_TemporaryActions)
			{
				action.Invoke();
			}

			s_unsetActions.Clear();
		}

		/// <summary>
		/// 등록.
		/// </summary>
		private static void Register<T>()
		{
			var key = typeof(T);
			if (s_unsetActions.ContainsKey(key))
				return;

			s_unsetActions.Add(key, Container<T>.Unset);
		}

		/// <summary>
		/// 등록 해제.
		/// </summary>
		private static void Unregister<T>()
		{
			var key = typeof(T);
			SharedInstances.Unregister(key);
		}

		/// <summary>
		/// 값 설정.
		/// </summary>
		public static void Set<T>(T instance)
		{
			Container<T>.Set(instance);
			Register<T>();
		}

		/// <summary>
		/// 값 비우기.
		/// </summary>
		public static void Unset<T>()
		{
			Container<T>.Unset();
			Unregister<T>();
		}

		/// <summary>
		/// 가져오기.
		/// </summary>
		public static T Get<T>()
		{
			return Container<T>.Get();
		}

		/// <summary>
		/// 가져오기.
		/// </summary>
		public static bool TryGet<T>(out T instance)
		{
			if (SharedInstances.IsSet<T>())
			{
				instance = SharedInstances.Get<T>();
				return true;
			}

			instance = default;
			return false;
		}

		/// <summary>
		/// 값 존재 유무.
		/// </summary>
		public static bool IsSet<T>()
		{
			return Container<T>.IsSet();
		}
	}
}