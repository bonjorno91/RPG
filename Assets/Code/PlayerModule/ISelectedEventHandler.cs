using Code.Selection.Events;
using Code.Services.EventService;

namespace Code.PlayerModule
{
    public interface ISelectedEventHandler<in TSelectable> : IEventHandler<IOnSelected<TSelectable>>
    {
        
    }
}