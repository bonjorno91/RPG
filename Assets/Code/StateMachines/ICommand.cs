namespace Code.StateMachines
{
    internal interface ICommand<out TState> where TState : IStateExitable
    {
        void Execute(IStateMachine<TState> stateMachine);
    }
}