using System;


namespace Crockhead.Core
{
	/// <summary>
	/// 결과가 포함된 명령.
	/// </summary>
	public class Operation<TResult> : Operation
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
			SetResult(default);
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
			m_Result = default;

			base.OnDispose(explicitDisposing);
		}

		/// <summary>
		/// 명령 재사용 설정.
		/// </summary>
		public override void Reset()
		{
			base.Reset();

			SetResult(default);
		}

		/// <summary>
		/// 명령 설정.
		/// </summary>
		public void SetOperation(Action<Operation<TResult>> operation)
		{
			base.SetOperation((Action<Operation>)operation);
		}

		/// <summary>
		/// 명령 설정.
		/// </summary>
		public void SetCompletion(Action<Operation<TResult>> completion)
		{
			base.SetCompletion((Action<Operation>)completion);
		}

		/// <summary>
		/// 결과 설정.
		/// </summary>
		protected void SetResult(TResult result)
		{
			m_Result = result;
		}

		/// <summary>
		/// 명령 성공.
		/// </summary>
		public void Success(TResult result)
		{
			SetResult(result);
			base.Success();
		}
	}
}