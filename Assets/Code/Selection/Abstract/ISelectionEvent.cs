using Code.Services.EventService;

namespace Code.Selection.Abstract
{
    public interface ISelectionEvent<out TSelectable> : IEvent 
    {
        TSelectable Selectable { get; }
    }
}