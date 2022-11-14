namespace Code.Services.EventService
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        void Handle(TEvent triggeredEvent);
    }
}