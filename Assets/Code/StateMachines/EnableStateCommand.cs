using Code.Abstract;

namespace Code.StateMachines
{
    internal class EnableStateCommand<TState> : ICommand where TState : class, IState
    {
        private readonly IStateMachine<TState> _machine;

        public EnableStateCommand(IStateMachine<TState> machine) => _machine = machine;

        public void Execute()
        {
            _machine.EnterState<TState>();
        }
    }
    
    internal class EnableStateCommand<TState, TPayload> : ICommand where TState : class, IStatePayload<TPayload>
    {
        private readonly IStateMachine<TState> _machine;
        private readonly TPayload _payload;

        public EnableStateCommand(IStateMachine<TState> machine, TPayload payload)
        {
            _payload = payload;
            _machine = machine;
        }

        public void Execute()
        {
            _machine.EnterState<TState,TPayload>(_payload);
        }
    }

}