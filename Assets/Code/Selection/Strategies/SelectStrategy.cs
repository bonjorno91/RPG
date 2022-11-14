using Code.PlayerModule;
using Code.Selection.Events;
using Code.Services.EventService;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Selection.Strategies
{
    /// <summary>
    /// Handle selecting entities with mouse.
    /// </summary>
    public class SelectStrategy : IPlayerInputStrategy
    {
        private class OnDragSelection : IOnDragSelectionBegin, IOnDragSelectionEnd, IOnRightClickOrder, IOnDragSelectionCancel
        {
            public Vector3 PointerPosition { get; set; }
        }

        private readonly OnDragSelection _event;
        private readonly IEventInvoker _eventInvoker;

        public SelectStrategy(IEventInvoker eventInvoker)
        {
            _eventInvoker = eventInvoker;
            _event = new OnDragSelection();
        }

        public void Handle(IPlayer player)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            _event.PointerPosition = Input.mousePosition;
         
            if (Input.GetMouseButtonDown(0))
            {
                _eventInvoker.Invoke<IOnDragSelectionBegin>(_event);
            }
            else if (Input.GetMouseButton(0))
            {
                _eventInvoker.Invoke<IOnDragSelectionHandled>(_event);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _eventInvoker.Invoke<IOnDragSelectionEnd>(_event);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                _eventInvoker.Invoke<IOnRightClickOrder>(_event);
            }
        }

        public void Cancel()
        {
            if (Input.GetMouseButton(0)) _eventInvoker.Invoke<IOnDragSelectionCancel>(_event);
        }
    }

}