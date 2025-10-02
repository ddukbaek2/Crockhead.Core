using System;


namespace Crockhead.Core
{
	/// <summary>
	/// 결과가 포함된 명령.
	/// </summary>
	public class Operation<TResult> : Operation, IOperation<TResult>
	{
		/// <summary>
		/// 결과.
		/// </summary>
		private TResult m_Result;

		/// <summary>
		/// 결과 프로퍼티.
		/// </summary>
		public TResult Result
		{
			get
			{
				if (!IsCompleted) throw new InvalidOperationException("Operation not completed.");
				if (!IsSucceeded) throw new InvalidOperationException("Operation failed or cancelled.", Exception);
				
				return m_Result;
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Operation() : base(null, null)
		{			
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Operation(Action<Operation<TResult>> operation) : this(operation, null)
		{
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public Operation(Action<Operation<TResult>> operation, Action<Operation<TResult>> completion) : 
			base(op => operation?.Invoke((Operation<TResult>)op), op => completion?.Invoke((Operation<TResult>)op))
		{
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			base.OnDispose(explicitDisposing);
		}

		/// <summary>
		/// 재초기화됨.
		/// </summary>
		protected override void OnReset()
		{
			SetResult(default);
			base.OnReset();
		}

		/// <summary>
		/// 명령 설정. (오버로딩)
		/// </summary>
		public void SetOperation(Action<Operation<TResult>> operation)
		{
			// 명령 실행 중에는 변경 불가.
			if (IsRunning)
				return;

			base.SetOperation((Action<IOperation>)operation);
		}

		/// <summary>
		/// 명령 설정. (오버로딩)
		/// </summary>
		public void SetCompletion(Action<Operation<TResult>> completion)
		{
			// 명령 실행 중에는 변경 불가.
			if (IsRunning)
				return;

			base.SetCompletion((Action<IOperation>)completion);
		}

		/// <summary>
		/// 결과 설정.
		/// </summary>
		private void SetResult(TResult result)
		{
			m_Result = result;
		}

		/// <summary>
		/// 성공. (사용금지)
		/// </summary>
		[Obsolete("Use Success(TResult result) instead. The old Success() method does not allow setting a result and is deprecated.", true)]
		public new void Success()
		{
			//base.Success();
			throw new NotSupportedException("Use Success(TResult) instead.");
		}

		/// <summary>
		/// 성공. (오버로딩)
		/// </summary>
		public void Success(TResult result)
		{
			// 명령이 시작되지 않았거나, 명령이 완료된 후에는 호출 불가.
			if (!IsStarted || IsCompleted)
				return;

			SetResult(result);
			base.Success();
		}
	}
}