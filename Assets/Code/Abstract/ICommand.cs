using Code.Services.CommandBus;
using Code.Services.EventService;

namespace Code.Abstract
{
    public interface ICommand : ICommandBase
    {
        void Execute();
    }

    public interface ICommand<in TPayload>: ICommandBase
    {
        void Execute(TPayload payload);
    }
}