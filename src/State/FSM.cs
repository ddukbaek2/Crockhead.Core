using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;


namespace Crockhead.Core
{
	/// <summary>
	/// 유한 상태 기계.
	/// <para>상태 전환시 OnTransition() ==> OnState() 호출됨.</para>
	/// </summary>
	public class FSM<TState> : Disposable
	{
		/// <summary>
		/// 상태.
		/// </summary>
		private TState m_State;

		/// <summary>
		/// 외부 전환 이벤트 연결 프로퍼티.
		/// </summary>
		public Action<TState, TState> OnTransitionEvent { set; get; }

		/// <summary>
		/// 외부 실행 이벤트 연결 프로퍼티.
		/// </summary>
		public Action<TState> OnStateEvent { set; get; }

		/// <summary>
		/// 상태 프로퍼티.
		/// </summary>
		public TState State
		{
			set
			{
				// 동일 상태의 재실행은 현재 프로퍼티가 아닌 SetState(state, true) 혹은 DoState()로 실행.
				// 현재 프로퍼티는 실제 달라진 상태로 변환되는 일반적인 동작만 허용.
				SetState(value, false);
			}
			get
			{
				return m_State;
			}
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public FSM(TState defaultState) : base()
		{
			// 실제 상태 이벤트의 호출이 아닌 기본값의 설정이므로 SetState()로 실행하지 않음.
			m_State = defaultState;

			OnTransitionEvent = null;
			OnStateEvent = null;
		}

		/// <summary>
		/// 해제됨.
		/// </summary>
		protected override void OnDispose(bool explicitDisposing)
		{
		}

		/// <summary>
		/// 상태 전환됨.
		/// </summary>
		protected virtual void OnTransition(TState previous, TState next)
		{
			OnTransitionEvent?.Invoke(previous, next);
		}

		/// <summary>
		/// 상태 실행됨.
		/// </summary>
		protected virtual void OnState(TState state)
		{
			OnStateEvent?.Invoke(state);
		}

		/// <summary>
		/// 상태 설정.
		/// </summary>
		public void SetState(TState state, bool forced = false)
		{
			// 기본 상태는 제외.
			var isDefaultState = EqualityComparer<TState>.Default.Equals(state, default);
			if (isDefaultState && !forced)
				return;

			// 동일 상태도 제외.
			var isSameState = EqualityComparer<TState>.Default.Equals(m_State, state);
			if (isSameState && !forced)
				return;

			var transitionException = default(ExceptionDispatchInfo);
			var stateException = default(ExceptionDispatchInfo);

			// 상태가 변경 된 경우만 전환 이벤트 호출.
			// 강제적으로 실행 했더라도 실제 변경은 없으므로 전환 이벤트는 호출하지 않음.
			if (!isSameState)
			{
				var previous = m_State;
				var next = state;
				m_State = state;

				try
				{
					OnTransition(previous, next);
				}
				catch (Exception exception)
				{
					//throw;
					transitionException = ExceptionDispatchInfo.Capture(exception);
				}
			}

			// 상태가 변경 되었거나 강제로 실행 했을 경우 실행 이벤트 호출.
			if (!isSameState || forced)
			{
				try
				{
					OnState(m_State);
				}
				catch (Exception exception)
				{
					//throw;
					stateException = ExceptionDispatchInfo.Capture(exception);
				}
			}

			if (transitionException != null && stateException != null)
			{
				throw new AggregateException(transitionException.SourceException, stateException.SourceException);
			}
			else if (transitionException != null)
			{
				transitionException.Throw();
			}
			else if (stateException != null)
			{
				stateException.Throw();
			}
		}

		/// <summary>
		/// 상태 재실행.
		/// </summary>
		public void DoState()
		{
			// 동일 상태에서 실행 이벤트를 호출하기 위함이므로 강제적인 상태 재설정.
			SetState(m_State, true);
		}
	}
}