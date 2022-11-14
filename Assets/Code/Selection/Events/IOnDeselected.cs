using Code.PlayerModule;
using Code.Selection.Abstract;

namespace Code.Selection.Events
{
    /// <summary>
    /// An event occurs when selectable entity was deselected by player.
    /// </summary>
    /// <typeparam name="TSelectable">Selectable type.</typeparam>
    public interface IOnDeselected<out TSelectable> : ISelectionEvent<TSelectable> 
    {
        IPlayer Player { get; }
    }
}