using System;
using System.Collections.Generic;

namespace Code.StateMachines
{
    public class StateMachine<TStateBase> : StateMachineBase<TStateBase> where TStateBase : IStateExitable
    {
        private readonly Action<IStateMachine<TStateBase>,Dictionary<Type, TStateBase>> _onRegisterStates;

        public StateMachine(Action<IStateMachine<TStateBase>, Dictionary<Type, TStateBase>> onRegisterStates) =>
            onRegisterStates?.Invoke(this, _states);
    }
}