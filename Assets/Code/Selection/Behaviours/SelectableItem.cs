using Code.Abstract;
using Code.EntityModule;
using Code.PlayerModule;
using Code.Selection.Abstract;
using UnityEngine;

namespace Code.Selection.Behaviours
{
    [RequireComponent(typeof(Item))]
    public class SelectableEntityBaseItem : SelectableEntity<Item>, ISelectableEntity
    {
        public override void VisitSelection(ISelectionVisitor selectionVisitor)
        {
            selectionVisitor.Select(_selectable);
        }

        public override void HandleCommand(ICommand command)
        {
            
        }
    }
}