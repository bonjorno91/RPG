using Code.Services.EventService;

namespace Code.Highlight
{
    public class PointerOverEvent<T> : IEvent
    {
        public T Hovered { get; }

        public PointerOverEvent(T hovered) => Hovered = hovered;
    }
}