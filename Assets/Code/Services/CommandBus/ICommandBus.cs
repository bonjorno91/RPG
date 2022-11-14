namespace Code.Services.CommandBus
{
    public interface ICommandBus : ICommandDispatcher
    {
        void Bind<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommandBase;
        void Unbind<TCommand>() where TCommand : ICommandBase;
    }
}