using Code.PlayerModule;
using Code.Selection.Abstract;
using Code.Services.EventService;

namespace Code.Selection.Events
{
    /// <summary>
    /// An event occurs when drag selection was active.
    /// </summary>
    public interface IOnSelectionBox : IEvent
    {
        ISelectableEntity Selectable { get; }
    }
}