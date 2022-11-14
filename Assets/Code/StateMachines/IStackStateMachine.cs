namespace Code.StateMachines
{
    public interface IStackStateMachine<in TStateExitable> : IStateMachine<TStateExitable> where TStateExitable : IStateExitable
    {
        void Push<TState>() where TState : class, IState, TStateExitable;
        void Push<TState, TPayload>(TPayload payload) where TState : class, IStatePayload<TPayload>, TStateExitable;
        void Pop();
    }
}