using Code.EntityModule;
using Code.PlayerModule;
using Code.Selection.Abstract;
using Code.Services.EventService;

namespace Code.Selection.Events
{
    public interface ISelectionEvent : IEvent
    {
        IEntity SelectedEntity { get; }
        IPlayer SelectedPlayer { get; }
    }
}