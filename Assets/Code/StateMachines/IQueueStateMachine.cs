namespace Code.StateMachines
{
    public interface IQueueStateMachine<in TStateExitable> : IStateMachine<TStateExitable> where TStateExitable : IStateExitable
    {
        void Enqueue<TState>() where TState : class, IState, TStateExitable;

        void Enqueue<TState, TPayload>(TPayload payload)
            where TState : class, IStatePayload<TPayload>, TStateExitable;

        void Dequeue();
    }
}