using System;
using System.Collections.Generic;

namespace Code.StateMachines
{
    public abstract class StateMachineBase<TStateExitable> : IStateMachine<TStateExitable>
        where TStateExitable : IStateExitable
    {
        private const int DefaultCapacity = 20;
        protected readonly Dictionary<Type, TStateExitable> _states;
        protected internal TStateExitable CurrentState { get; private set; }

        protected StateMachineBase(int startStateCapacity = DefaultCapacity)
        {
            _states = new Dictionary<Type, TStateExitable>(startStateCapacity);
        }
        
        public void EnterState<TState>() where TState : class, IState, TStateExitable =>
            ChangeState<TState>()?.OnEnter();

        public void EnterState<TState, TPayload>(TPayload payload)
            where TState : class, IStatePayload<TPayload>, TStateExitable =>
            ChangeState<TState>()?.OnEnter(payload);

        private T ChangeState<T>() where T : class, TStateExitable
        {
            var next = GetState<T>();
            CurrentState?.OnExit();
            CurrentState = next;
            return next;
        }

        private T GetState<T>() where T : class, TStateExitable => _states[typeof(T)] as T;
    }
}