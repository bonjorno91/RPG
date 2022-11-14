using Code.Services.CommandBus;

namespace Code.Highlight
{
    public class HighlightCommandHandler : ICommandHandler<HighlightCommand>
    {
        public bool Handle(HighlightCommand command)
        {
            return false;
        }

        public void OnUnbind()
        {
            throw new System.NotImplementedException();
        }
    }
}