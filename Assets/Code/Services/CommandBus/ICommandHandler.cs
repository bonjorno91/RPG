namespace Code.Services.CommandBus
{
    /// <summary>
    /// Command Bus handler interface.
    /// </summary>
    /// <typeparam name="TCommand">ICommandMark command.</typeparam>
    public interface ICommandHandler<in TCommand> where TCommand : ICommandBase
    {
        /// <summary>
        /// Handle command operation.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <returns>Command operation success result.</returns>
        bool Handle(TCommand command);
        
        /// <summary>
        /// Execute when someone unbind.
        /// </summary>
        void OnUnbind();
    }
}