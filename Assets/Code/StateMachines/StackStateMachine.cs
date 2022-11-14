using System;
using System.Collections.Generic;

namespace Code.StateMachines
{
    public class StackStateMachine<TStateBase> : StackStateMachineBase<TStateBase>
        where TStateBase : IStateExitable
    {
        private readonly Action<IStackStateMachine<TStateBase>, Dictionary<Type, TStateBase>> _onRegisterStates;

        protected StackStateMachine(
            Action<IStackStateMachine<TStateBase>, Dictionary<Type, TStateBase>> onRegisterStates, int capacity = 100) =>
            onRegisterStates?.Invoke(this,_states);
    }
}