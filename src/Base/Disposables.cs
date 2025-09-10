using System;
using IDotNetDisposable = System.IDisposable;


namespace Crockhead.Core
{
	/// <summary>
	/// 해제 가능한 객체의 주요 유틸리티.
	/// </summary>
	public static class Disposables
	{
		/// <summary>
		/// 객체 생성. (기본)
		/// <para>명시적 함수로 생성. (new TDisposable()로 생성해도 결과는 동일)</para>
		/// </summary>
		public static TDisposable Create<TDisposable>(params object[] arguments) where TDisposable : Disposable
		{
			var obj = Reflections.CreateInstance<TDisposable>(arguments);
			return obj;
		}

		/// <summary>
		/// 약한 참조로 객체 생성.
		/// </summary>
		public static WeakReference<TDisposable> CreateWeakReference<TDisposable>(params object[] arguments) where TDisposable : Disposable
		{
			var obj = Disposables.Create<TDisposable>(arguments);
			return new WeakReference<TDisposable>(obj);
		}

		/// <summary>
		/// 해제. (시스템)
		/// </summary>
		public static void DotNetDispose(IDotNetDisposable disposable)
		{
			if (disposable == null)
				return;

			disposable.Dispose();
		}

		/// <summary>
		/// 해제. (기본)
		/// </summary>
		public static void Dispose(IDisposable disposable)
		{
			if (Disposables.IsDisposed(disposable))
				return;

			//disposable.Dispose();
			Disposables.DotNetDispose(disposable);
		}


		/// <summary>
		/// 해제 여부 반환. (기본)
		/// </summary>
		public static bool IsDisposed(IDisposable disposable)
		{
			if (disposable == null)
				return true;

			return disposable.IsDisposed;
		}

		/// <summary>
		/// 아무 객체나 넣어서 해제 가능하면 해제.
		/// <para>해제되었거나, 해제하면 참 반환.</para>
		/// </summary>
		public static bool Dispose(object obj)
		{
			if (Disposables.IsDisposed(obj))
				return true;

			if (obj is IDisposable)
			{
				Disposables.Dispose((IDisposable)obj);
				return true;
			}
			else if (obj is IDotNetDisposable)
			{
				Disposables.DotNetDispose((IDotNetDisposable)obj);
				return true;
			}

			return false;
		}

		/// <summary>
		/// 아무 객체나 넣어서 해제 여부를 얻어낼 수 있으면 여부 반환.
		/// <para>객체가 null일 경우 해제된 것으로 판정.</para>
		/// <para>객체가 IDisposable의 IsDisposed 프로퍼티로 해제 여부 판정.</para>
		/// </summary>
		public static bool IsDisposed(object obj)
		{
			if (obj == null)
				return true;

			if (obj is IDisposable disposable)
			{
				return Disposables.IsDisposed(disposable);
			}
			else if (obj is IDotNetDisposable)
			{
				// 어차피 알 수가 없음.
			}

			return false;
		}

		/// <summary>
		/// 안전한 해제. (제너릭)
		/// <para>해제 후 null값을 대입.</para>
		/// </summary>
		public static void SafeDispose<TDispsoable>(ref TDispsoable disposable) where TDispsoable : IDisposable
		{
			Disposables.Dispose(disposable);
			disposable = default(TDispsoable);
		}
	}
}