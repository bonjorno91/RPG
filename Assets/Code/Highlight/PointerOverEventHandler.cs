using Code.Services.EventService;

namespace Code.Highlight
{
    public abstract class PointerOverEventHandler<T> : IEventHandler<PointerOverEvent<T>> 
    {
        public abstract void Handle(PointerOverEvent<T> triggeredEvent);
    }
}