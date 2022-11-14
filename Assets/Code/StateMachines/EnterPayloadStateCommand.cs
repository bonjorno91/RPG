using Code.Abstract;

namespace Code.StateMachines
{
    internal class EnterPayloadStateCommand<TState, TPayload> : ICommand<TState>
        where TState : class, IStatePayload<TPayload>
    {
        private readonly TPayload _payload;

        public EnterPayloadStateCommand(TPayload payload) => _payload = payload;

        public void Execute(IStateMachine<TState> stateMachine) =>
            stateMachine.EnterState<TState, TPayload>(_payload);
    }
}