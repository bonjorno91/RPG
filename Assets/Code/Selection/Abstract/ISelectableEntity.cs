using Code.Abstract;
using Code.EntityModule;
using Code.Highlight;
using Code.PlayerModule;
using UnityEngine;

namespace Code.Selection.Abstract
{
    public interface ISelectableEntity : IHighlightable
    {
        IEntity Entity { get; }
        Bounds DragSelectionBox { get; }
        Bounds ClickSelectionBox { get; }
        void VisitSelection(ISelectionVisitor selectionVisitor);
        void HandleCommand(ICommand command);
    }
}