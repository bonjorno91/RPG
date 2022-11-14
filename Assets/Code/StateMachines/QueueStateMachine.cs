using System;
using System.Collections.Generic;

namespace Code.StateMachines
{
    public class QueueStateMachine<TStateBase> : QueueStateMachineBase<TStateBase>
        where TStateBase : IStateExitable
    {
        private readonly Action<IQueueStateMachine<TStateBase>, Dictionary<Type, TStateBase>> _onRegisterStates;

        public QueueStateMachine(Action<IQueueStateMachine<TStateBase>, Dictionary<Type, TStateBase>> onRegisterStates)
        {
            onRegisterStates?.Invoke(this,_states);
        }
    }
}