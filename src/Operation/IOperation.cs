using System;

namespace Crockhead.Core
{
	/// <summary>
	/// 명령 인터페이스.
	/// </summary>
	public interface IOperation : IDisposable
	{
		/// <summary>
		/// 진행 중 프로퍼티.
		/// </summary>
		public bool IsRunning { get; }

		/// <summary>
		/// 시작 여부 프로퍼티.
		/// <para>한번이라도 Start()를 호출하면 이후 Reset() 전까지는 계속 참을 반환.</para>
		/// </summary>
		bool IsStarted { get; }

		/// <summary>
		/// 완료 여부 프로퍼티. (IsCompletedSuccessfully)
		/// <para>한번이라도 Complete() or Cancel()을 호출하면 이후 Reset() 전까지는 계속 참을 반환.</para>
		/// </summary>
		bool IsCompleted { get; }

		/// <summary>
		/// 완료 + 성공 여부 프로퍼티.
		/// </summary>
		bool IsSucceeded { get; }

		/// <summary>
		/// 완료 + 실패 여부 프로퍼티.
		/// </summary>
		bool IsFaulted { get; }

		/// <summary>
		/// 완료 + 취소 여부 프로퍼티.
		/// </summary>
		bool IsCanceled { get; }

		/// <summary>
		/// 실패 예외 프로퍼티.
		/// </summary>
		Exception Exception { get; }

		/// <summary>
		/// 재사용 할 수 있도록 초기화.
		/// </summary>
		void Reset();

		/// <summary>
		/// 명령 설정.
		/// </summary>
		void SetOperation(Action<IOperation> operation);

		/// <summary>
		/// 명령 설정.
		/// </summary>
		void SetCompletion(Action<IOperation> completion);

		/// <summary>
		/// 시작.
		/// </summary>
		void Start();

		/// <summary>
		/// 대기.
		/// </summary>
		void WaitForCompletion();

		/// <summary>
		/// 성공.
		/// </summary>
		void Success();

		/// <summary>
		/// 실패.
		/// </summary>
		void Fail(Exception exception);

		/// <summary>
		/// 취소.
		/// </summary>
		void Cancel();
	}
}