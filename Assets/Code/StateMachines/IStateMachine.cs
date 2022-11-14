namespace Code.StateMachines
{
    public interface IStateMachine<in TStateExitable> where TStateExitable : IStateExitable
    {
        void EnterState<TState>() where TState : class, IState, TStateExitable;

        void EnterState<TState, TPayload>(TPayload payload)
            where TState : class, IStatePayload<TPayload>, TStateExitable;
    }
}