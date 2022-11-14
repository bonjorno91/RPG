using Code.Selection.Events;
using Code.Services.EventService;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Selection
{
    /// <summary>
    /// Draw selection box on player screen.
    /// </summary>
    public class SelectionDragUIHandler : Image,
        IEventHandler<IOnDragSelectionBegin>,
        IEventHandler<IOnDragSelectionHandled>,
        IEventHandler<IOnDragSelectionEnd>
    {
        private Vector3 _startPoint;

        public void Handle(IOnDragSelectionBegin triggeredEvent)
        {
            _startPoint = triggeredEvent.PointerPosition;
            rectTransform.anchoredPosition = _startPoint;
            rectTransform.root.gameObject.SetActive(true);
        }

        public void Handle(IOnDragSelectionHandled triggeredEvent)
        {
            var difference = triggeredEvent.PointerPosition - _startPoint;
            var startPoint = _startPoint;

            if (difference.x < 0)
            {
                startPoint.x = triggeredEvent.PointerPosition.x;
                difference.x = -difference.x;
            }


            if (difference.y < 0)
            {
                startPoint.y = triggeredEvent.PointerPosition.y;
                difference.y = -difference.y;
            }


            rectTransform.anchoredPosition = startPoint;
            rectTransform.sizeDelta = difference;
        }

        public void Handle(IOnDragSelectionEnd triggeredEvent)
        {
            rectTransform.root.gameObject.SetActive(false);
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
        }
    }
}