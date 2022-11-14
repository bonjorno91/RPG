using System.Collections.Generic;
using System.Linq;
using Code.EntityModule;
using Code.PlayerModule;
using Code.Selection.Abstract;

namespace Code.Selection.Strategies
{
    public class SelectionGroupStrategy : ISelectionVisitor, ISelectionGroupFilterStrategy
    {
        private readonly List<Unit> _unitsGroup = new List<Unit>();
        private Item _item;
        private Building _building;
        private IPlayer _player;

        public void Filter(IPlayer player, IEnumerable<ISelectableEntity> selectables,
            ISelectionVisitor selectionHandler)
        {
            _player = player;

            foreach (var selectable in selectables)
            {
                selectable?.VisitSelection(this);
            }

            if (_unitsGroup?.Count > 0)
            {
                var relationPlayerUnits = from unit in _unitsGroup
                    group unit by unit.OwnerPlayerID
                    into playerUnitsGroup
                    group playerUnitsGroup by player.GetRelationsFor(playerUnitsGroup.Key);

                var playerUnits = from relationsGroup in relationPlayerUnits
                    where relationsGroup.Key == Relations.Ally
                    from playerUnitsGroup in relationsGroup
                    where playerUnitsGroup.Key == player.ID
                    from unit in playerUnitsGroup
                    select unit;

                if (playerUnits.Any())
                {
                    selectionHandler.Select(playerUnits);
                }
                else if (_building && _building.OwnerPlayerID == _player.ID)
                {
                    selectionHandler.Select(_building);
                }
                else
                {
                    var allyUnits = from relationGroup in relationPlayerUnits
                        where relationGroup.Key == Relations.Ally
                        from allyGroup in relationGroup
                        from unit in allyGroup
                        select unit;

                    if (allyUnits.Any())
                    {
                        selectionHandler.Select(allyUnits);
                    }
                    else
                    {
                        var neutralUnits = from relationsGroup in relationPlayerUnits
                            where relationsGroup.Key == Relations.Neutral
                            from neutralGroup in relationsGroup
                            from unit in neutralGroup
                            select unit;

                        if (neutralUnits.Any())
                        {
                            selectionHandler.Select(neutralUnits.First());
                        }
                        else
                        {
                            var enemyUnits = from relationPlayerUnit in relationPlayerUnits
                                where relationPlayerUnit.Key == Relations.Enemy
                                from enemyPlayer in relationPlayerUnit
                                from unit in enemyPlayer
                                select unit;

                            if (enemyUnits.Any()) selectionHandler.Select(enemyUnits.First());
                        }
                    }
                }
            }
            else if (_building)
            {
                selectionHandler.Select(_building);
            }
            else
            {
                selectionHandler.Select(_item);
            }

            _unitsGroup?.Clear();
            _item = null;
            _building = null;
        }

        public void Select(Unit unit)
        {
            if (unit) _unitsGroup.Add(unit);
        }

        public void Select(Item item)
        {
            if (item) _item = item;
        }

        public void Select(Building building)
        {
            if (building)
            {
                if (_building && _building.OwnerPlayerID == _player.ID) return;
                
                _building = building;
            }
        }

        public void Select(IEnumerable<Unit> unitsGroup)
        {
            foreach (var unit in unitsGroup)
            {
                Select(unit);
            }
        }

        public void DeselectFor(Unit unit, IPlayer player)
        {
            if (unit) _unitsGroup.Remove(unit);
        }

        public void DeselectFor(Item item, IPlayer player)
        {
            if (item && item == _item) _item = null;
        }

        public void DeselectFor(Building building, IPlayer player)
        {
            if (building && building == _building) _building = null;
        }
    }
}