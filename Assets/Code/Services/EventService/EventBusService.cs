using System;
using System.Collections.Generic;
using Code.Abstract;
using UnityEngine;

namespace Code.Services.EventService
{
    public class EventBusService : IEventBus
    {
        private readonly Queue<ICommand> _commandQueue = new();
        private readonly Dictionary<Type, List<object>> _eventHandlers = new();
        private Type _raiseEventType;

        public void Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            var eventType = typeof(TEvent);

            if (_raiseEventType == eventType)
            {
                _commandQueue.Enqueue(new EventActionCommand<TEvent>(Subscribe, handler));
                return;
            }

            if (!_eventHandlers.TryGetValue(eventType, out var list))
            {
                list = new List<object>();
                _eventHandlers[eventType] = list;
            }

            list.Add(handler);
        }
        
        public void Subscribe(Type eventType,object handler)
        {
            if (!_eventHandlers.TryGetValue(eventType, out var list))
            {
                list = new List<object>();
                _eventHandlers[eventType] = list;
            }

            list.Add(handler);
        }
        public void Unsubscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            var eventType = typeof(TEvent);

            if (_raiseEventType == eventType)
            {
                _commandQueue.Enqueue(new EventActionCommand<TEvent>(Unsubscribe, handler));
                return;
            }

            if (!_eventHandlers.TryGetValue(eventType, out var list))
            {
                list = new List<object>();
                _eventHandlers[eventType] = list;
            }

            list.Remove(handler);
        }

        public void Invoke<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (@event == null) return;

            if (!_eventHandlers.TryGetValue(typeof(TEvent), out var handlers)) return;

            _raiseEventType = typeof(TEvent);
            RaiseEvent(@event, handlers);
            _raiseEventType = default;
            DrainQueue();
        }

        private static void RaiseEvent<TEvent>(TEvent @event, List<object> handlers) where TEvent : IEvent
        {
            foreach (var handler in handlers) ((IEventHandler<TEvent>) handler).Handle(@event);
        }

        private void DrainQueue()
        {
            while (_commandQueue.TryDequeue(out var command)) command.Execute();
        }

        private class EventActionCommand<TEvent> : ICommand where TEvent : IEvent
        {
            private readonly Action<IEventHandler<TEvent>> _action;
            private readonly IEventHandler<TEvent> _handler;

            public EventActionCommand(Action<IEventHandler<TEvent>> action, IEventHandler<TEvent> handler)
            {
                _action = action;
                _handler = handler;
            }

            public void Execute()
            {
                _action?.Invoke(_handler);
            }
        }
    }
}