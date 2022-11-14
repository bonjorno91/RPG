using Code.PlayerModule;
using Code.Selection.Abstract;

namespace Code.Selection.Events
{
    /// <summary>
    /// An event occurs when selectable entity was selected by player.
    /// </summary>
    /// <typeparam name="TSelectable">Selectable type.</typeparam>
    public interface IOnSelected<out TSelectable> : ISelectionEvent<TSelectable> 
    {
        IPlayer Player { get; }
    }
}