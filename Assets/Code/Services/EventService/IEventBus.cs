using System;

namespace Code.Services.EventService
{
    public interface IEventBus : IEventInvoker
    {
        void Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent;
        void Unsubscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent;
        void Subscribe(Type eventType,object handler);
    }
}