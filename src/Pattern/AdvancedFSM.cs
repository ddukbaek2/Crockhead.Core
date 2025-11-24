using System;


namespace Crockhead.Core
{
	/// <summary>
	/// 좀 더 고도화된 유한 상태 기계. (FiniteStateMachine)
	/// </summary>
	public abstract class AdvancedFSM<TState> : FSM<TState>
	{
		/// <summary>
		/// 외부 진입 이벤트 연결 프로퍼티.
		/// </summary>
		public Action<TState, TState> OnEnterEvent { set; get; }

		/// <summary>
		/// 외부 갱신 이벤트 연결 프로퍼티.
		/// </summary>
		public Action<TState> OnUpdateEvent { set; get; }

		/// <summary>
		/// 외부 탈출 이벤트 연결 프로퍼티.
		/// </summary>
		public Action<TState, TState> OnExitEvent { set; get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public AdvancedFSM(TState initialState) : base(initialState)
		{
			//OnDecideEvent = null;
			OnEnterEvent = null;
			OnUpdateEvent = null;
			OnExitEvent = null;
		}

		/// <summary>
		/// 생성됨.
		/// </summary>
		public AdvancedFSM() : this(default)
		{
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
		protected sealed override void OnTransition(TState previous, TState next)
		{
			base.OnTransition(previous, next);

			OnExit(previous, next);
			OnEnter(previous, next);
		}

		/// <summary>
		/// 상태 실행됨.
		/// </summary>
		protected sealed override void OnState(TState state)
		{
			base.OnState(state);

			OnUpdate(state);
		}

		/// <summary>
		/// 다음 상태를 결정함.
		/// </summary>
		protected abstract TState OnDecide(TState current);

		/// <summary>
		/// 현재 상태 시작됨.
		/// </summary>
		protected virtual void OnEnter(TState previous, TState current)
		{
			OnEnterEvent?.Invoke(previous, current);
		}

		/// <summary>
		/// 현재 상태 갱신됨.
		/// </summary>
		protected virtual void OnUpdate(TState current)
		{
			OnUpdateEvent?.Invoke(current);
		}

		/// <summary>
		/// 현재 상태 탈출됨.
		/// </summary>
		protected virtual void OnExit(TState current, TState next)
		{
			OnExitEvent?.Invoke(current, next);
		}

		/// <summary>
		/// 상태 머신 갱신. (외부에서 호출 필수)
		/// </summary>
		public virtual void Update()
		{
			var current = State;
			var next = OnDecide(current);

			var isSameState = AdvancedFSM<TState>.Equals(current, next);
			
			// 상태 변경.
			base.SetState(next, isSameState);
		}

		/// <summary>
		/// 상태 재실행.
		/// </summary>
		public sealed override void DoState()
		{
			// OnEnter/OnUpdate/OnExit 구조에 영향을 줄 수 있는 동일 상태 재실행 기능 무시.
			//base.DoState();
		}


		/// <summary>
		/// 상태 설정.
		/// </summary>
		public sealed override void SetState(TState state, bool forced = false)
		{
			// 매 프레임마다 갱신되며 호출되는 OnDecide를 통해서만 상태가 변경되도록 기존 상태 설정 기능 무시.
			//base.SetState(state, forced);
		}
	}
}