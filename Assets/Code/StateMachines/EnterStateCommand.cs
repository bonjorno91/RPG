namespace Code.StateMachines
{
    internal class EnterStateCommand<TState> : ICommand<TState> where TState : class, IState
    {
        public void Execute(IStateMachine<TState> stateMachine) => stateMachine.EnterState<TState>();
    }
}