using Code.Abstract;
using Code.Services.CommandBus;
using Code.Services.EventService;

namespace Code.Events
{
    public class OnMouseOverEnter<T> : IEvent
    {
        
    }
    
    public class OnMouseOverExit<T> : IEvent
    {
        
    }
    
    public class OnMouseOverHandler<T> : 
        IEventHandler<OnMouseOverEnter<T>>,
        IEventHandler<OnMouseOverExit<T>>
    {
        private readonly ICommandBus _commandBus;
        private readonly ICommand _onExitCommand;
        private readonly ICommand _onEnterCommand;

        public OnMouseOverHandler(ICommandBus commandBus, ICommand onEnterCommand,ICommand onExitCommand)
        {
            _commandBus = commandBus;
            _onEnterCommand = onEnterCommand;
            _onExitCommand = onExitCommand;
        }

        public void Handle(OnMouseOverEnter<T> triggeredEvent) => _commandBus.Dispatch(_onEnterCommand);

        public void Handle(OnMouseOverExit<T> triggeredEvent) => _commandBus.Dispatch(_onExitCommand);
    }
}