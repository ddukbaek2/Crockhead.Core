using System;
using IDotNetDisposable = System.IDisposable;


namespace Crockhead.Core
{
	/// <summary>
	/// 해제 가능한 객체.
	/// <para>명시적으로 내부 자원의 사용 해제 구간이 존재하는 객체를 의미.</para>
	/// <para>단, Dispose()가 호출되더라도 이것이 객체 자체의 제거를 의미하진 않으므로 실제 객체가 null이 되려면 이후 모든 참조를 끊어야 한다.</para>
	/// <para>IDotNetDisposable 인터페이스 구현체.</para>
	/// <para>IDisposable 인터페이스 구현체.</para>
	/// <para>ObjectDisposedException는 일단 사용하지 않음.</para>
	/// </summary>
	public abstract class Disposable : IDisposable
	{
		/// <summary>
		/// 해제 되었는지 여부.
		/// </summary>
		private bool m_IsDisposed;

		/// <summary>
		/// 해제 되었는지 여부 프로퍼티.
		/// <para>IDisposable 인터페이스 구현.</para>
		/// </summary>
		public bool IsDisposed => m_IsDisposed;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Disposable()
		{
			m_IsDisposed = false;
		}

		/// <summary>
		/// 소멸됨.
		/// </summary>
		~Disposable()
		{
			if (m_IsDisposed)
				return;

			try
			{
				m_IsDisposed = true;
				OnDispose(false);
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		/// <param name="explicitDisposing">명시적인 해제 여부. (직접적인 Dispose() 호출을 통한 해제)</param>
		protected abstract void OnDispose(bool explicitDisposing);

		/// <summary>
		/// 해제.
		/// <para>IDotNetDisposable 인터페이스 구현.</para>
		/// </summary>
		void IDotNetDisposable.Dispose()
		{
			if (m_IsDisposed)
				return;

			try
			{
				m_IsDisposed = true;
				GC.SuppressFinalize(this); // 소멸자 실행 중단.
				OnDispose(true);
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}
	}
}