using Code.Services.EventService;
using UnityEngine;

namespace Code.Selection.Events
{
    /// <summary>
    /// An event occurs when box drag selection is handling.
    /// </summary>
    public interface IOnDragSelectionHandled : IEvent
    {
        Vector3 PointerPosition { get; }
    }
}