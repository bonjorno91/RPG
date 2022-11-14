using System.Collections.Generic;
using Code.EntityModule;
using Code.PlayerModule;

namespace Code.Selection.Abstract
{
    public interface ISelectionVisitor
    {
        /// <summary>
        /// Select unit for player.
        /// </summary>
        /// <param name="unit"></param>
        void Select(Unit unit);

        /// <summary>
        /// Select item for player.
        /// </summary>
        /// <param name="item"></param>
        void Select(Item item);

        /// <summary>
        /// Select building for player.
        /// </summary>
        /// <param name="building"></param>
        void Select(Building building);

        void Select(IEnumerable<Unit> unitsGroup);
    }
}