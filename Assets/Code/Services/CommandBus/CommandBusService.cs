using System;
using System.Collections.Generic;

namespace Code.Services.CommandBus
{
    public sealed class CommandBusService : ICommandBus
    {
        private readonly Dictionary<Type, object> _commandBinds = new();

        public void Bind<TCommand>(ICommandHandler<TCommand> handler) where TCommand : ICommandBase
        {
            _commandBinds[typeof(TCommand)] = handler;
        }

        public void Unbind<TCommand>() where TCommand : ICommandBase
        {
            var type = typeof(TCommand);

            if (_commandBinds.TryGetValue(type, out var commandHandler))
            {
                if (commandHandler is ICommandHandler<TCommand> handler)
                    handler.OnUnbind();
            }

            _commandBinds[type] = default;
        }

        public bool Dispatch<TCommand>(TCommand command) where TCommand : class, ICommandBase
        {
            if (_commandBinds.TryGetValue(typeof(TCommand), out var handler))
            {
                if (handler is ICommandHandler<TCommand> commandHandler)
                {
                    return commandHandler.Handle(command);
                }
            }

            return false;
        }
    }
}