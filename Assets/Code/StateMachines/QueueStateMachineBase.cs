using System.Collections.Generic;

namespace Code.StateMachines
{
    public abstract class QueueStateMachineBase<TStateExitable> :
        StateMachineBase<TStateExitable>,
        IQueueStateMachine<TStateExitable>
        where TStateExitable : IStateExitable
    {
        private const int DefaultCapacity = 20;
        private readonly Queue<ICommand<TStateExitable>> _commandQueue;

        protected QueueStateMachineBase(int queueCapacity = DefaultCapacity)
        {
            _commandQueue = new Queue<ICommand<TStateExitable>>(queueCapacity);
        }

        public void Enqueue<TState>() where TState : class, IState, TStateExitable
        {
            _commandQueue.Enqueue(new EnterStateCommand<TState>());
        }

        public void Enqueue<TState, TPayload>(TPayload payload)
            where TState : class, IStatePayload<TPayload>, TStateExitable
        {
            _commandQueue.Enqueue(new EnterPayloadStateCommand<TState, TPayload>(payload));
        }

        public void Dequeue()
        {
            _commandQueue.Dequeue()?.Execute(this);
        }
    }
}