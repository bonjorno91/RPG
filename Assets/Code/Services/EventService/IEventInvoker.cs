using System;

namespace Code.Services.EventService
{
    public interface IEventInvoker
    {
        void Invoke<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}