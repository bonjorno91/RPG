namespace Code.StateMachines
{
    public interface IState : IStateExitable
    {
        void OnEnter();
    }
}