using Code.Selection.Events;
using Code.Services.EventService;

namespace Code.PlayerModule
{
    public interface IDeselectedEventHandler<in TSelectable> : IEventHandler<IOnDeselected<TSelectable>>
    {
        
    }
}