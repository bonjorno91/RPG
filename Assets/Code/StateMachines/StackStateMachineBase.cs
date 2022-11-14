using System.Collections.Generic;

namespace Code.StateMachines
{
    public abstract class StackStateMachineBase<TStateExitable> : 
        StateMachineBase<TStateExitable>,
        IStackStateMachine<TStateExitable>
        where TStateExitable : IStateExitable
    {
        private const int DefaultCapacity = 100;
        private readonly Stack<ICommand<TStateExitable>> _commandStack;

        protected StackStateMachineBase(int capacity = DefaultCapacity)
        {
            _commandStack = new Stack<ICommand<TStateExitable>>(capacity);
        }

        public void Push<TState>() where TState : class, IState, TStateExitable
        {
            _commandStack.Push(new EnterStateCommand<TState>());
        }

        public void Push<TState, TPayload>(TPayload payload)
            where TState : class, IStatePayload<TPayload>, TStateExitable
        {
            _commandStack.Push(new EnterPayloadStateCommand<TState, TPayload>(payload));
        }

        public void Pop()
        {
            _commandStack.Pop()?.Execute(this);
        }
    }
}