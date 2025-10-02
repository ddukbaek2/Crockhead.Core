using System;

namespace Crockhead.Core
{
	/// <summary>
	/// 명령 인터페이스.
	/// </summary>
	public interface IOperation : IDisposable
	{
		/// <summary>
		/// 시작 여부 프로퍼티.
		/// <para>Start() 호출 이후 Reset() 호출 전까지는 계속 True.</para>
		/// </summary>
		bool IsStarted { get; }

		/// <summary>
		/// 진행 중 여부 프로퍼티.
		/// <para>IsStarted + IsCompleted</para>
		/// </summary>
		bool IsRunning { get; }

		/// <summary>
		/// 완료 여부 프로퍼티. (=Task.IsCompleted)
		/// <para>Start() 호출 이후 Success(), Fail(), Cancel() 호출 이후 Reset() 호출 전까지는 계속 True.</para>
		/// </summary>
		bool IsCompleted { get; }

		/// <summary>
		/// 완료 + 성공 여부 프로퍼티. (=Task.IsCompletedSuccessfully)
		/// <para>Success() 호출 이후 True.</para>
		/// </summary>
		bool IsSucceeded { get; }

		/// <summary>
		/// 완료 + 실패 여부 프로퍼티. (=Task.IsFaulted)
		/// <para>Fail() 호출 이후 True.</para>
		/// </summary>
		bool IsFaulted { get; }

		/// <summary>
		/// 완료 + 취소 여부 프로퍼티. (=Task.IsCanceled)
		/// <para>Cancel() 호출 이후 True.</para>
		/// </summary>
		bool IsCanceled { get; }

		/// <summary>
		/// 실패 시 예외 프로퍼티.
		/// <para>Fail() 호출시 입력한 Exception.</para>
		/// </summary>
		Exception Exception { get; }

		/// <summary>
		/// 상태 프로퍼티.
		/// <para>IsStarted, IsCompleted, IsSucceeded, IsFaulted, IsCanceled에 의한 현재 상황.</para>
		/// </summary>
		OperationStatus Status { get; }

		/// <summary>
		/// 명령 설정.
		/// </summary>
		void SetOperation(Action<IOperation> operation);

		/// <summary>
		/// 명령 설정.
		/// </summary>
		void SetCompletion(Action<IOperation> completion);

		/// <summary>
		/// 재초기화.
		/// </summary>
		void Reset();

		/// <summary>
		/// 시작.
		/// </summary>
		void Start();

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