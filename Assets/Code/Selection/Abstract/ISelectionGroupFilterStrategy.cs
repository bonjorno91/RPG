using System.Collections.Generic;
using Code.PlayerModule;

namespace Code.Selection.Abstract
{
    public interface ISelectionGroupFilterStrategy
    {
        void Filter(IPlayer forPlayer, IEnumerable<ISelectableEntity> selectables, ISelectionVisitor selectionHandler);
    }
}