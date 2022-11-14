namespace Code.Services.CommandBus
{
    public interface ICommandDispatcher
    {
        bool Dispatch<TCommand>(TCommand command) where TCommand : class, ICommandBase;
    }
}