using System.Collections.Generic;
using System.Linq;
using Code.Abstract;
using Code.Behaviours;
using Code.EntityModule;
using Code.Installers;
using Code.PlayerModule;
using Code.Selection.Abstract;
using Code.Selection.Events;
using Code.Services.EventService;
using Zenject;

namespace Code.Selection
{
    public class SelectionHighlightHandler : IEventHandler<IOnDragSelectionEnter>, IEventHandler<IOnDragSelectionExit>
    {
        private readonly Dictionary<IEntity, SelectionCircle> _activeHUDs = new();
        private readonly SelectionMarksPool _selectionMarksPool;
        private readonly GroupHighlightStrategy _highlightStrategy;

        public SelectionHighlightHandler(
            [Inject(Id = BindID.LocalPlayer)] IPlayer localPlayer,
            SelectionMarksPool selectionMarksPool)
        {
            _selectionMarksPool = selectionMarksPool;
            _highlightStrategy = new GroupHighlightStrategy(this, localPlayer);
        }

        public void Handle(IOnDragSelectionEnter triggeredEvent) => _highlightStrategy.Handle(triggeredEvent);

        public void Handle(IOnDragSelectionExit triggeredEvent) => _highlightStrategy.Handle(triggeredEvent);

        public void Enable(IEntity entity, Relations relation)
        {
            var instance = _selectionMarksPool.Get(relation);

            if (_activeHUDs.TryAdd(entity, instance))
                instance.Highlight(entity.Origin);
            else
                _selectionMarksPool.Release(instance);
        }

        public void Disable(IEntity entity)
        {
            if (_activeHUDs.Remove(entity, out var instance))
            {
                _selectionMarksPool.Release(instance);
            }
        }
    }

    internal class GroupHighlightStrategy : IEventHandler<IOnDragSelectionEnter>, IEventHandler<IOnDragSelectionExit>
    {
        private readonly SelectionHighlightHandler _highlightHandler;
        private readonly IPlayer _localPlayer;
        private readonly HashSet<ISelectableEntity> _selectableEntities;
        private readonly HashSet<ISelectableEntity> _highlightedEntities;

        public GroupHighlightStrategy(SelectionHighlightHandler highlightHandler, IPlayer localPlayer)
        {
            _localPlayer = localPlayer;
            _highlightHandler = highlightHandler;
            _selectableEntities = new HashSet<ISelectableEntity>();
            _highlightedEntities = new HashSet<ISelectableEntity>();
        }

        public void Handle(IOnDragSelectionEnter triggeredEvent)
        {
            if (triggeredEvent.Selectable == null) return;

            _selectableEntities.Add(triggeredEvent.Selectable);

            Filter();
        }

        public void Handle(IOnDragSelectionExit triggeredEvent)
        {
            if (triggeredEvent.Selectable.Entity == null) return;

            if (_selectableEntities.Remove(triggeredEvent.Selectable))
            {
                Filter();
            }
        }

        private void Filter()
        {
            var playerEntities = from selectableEntity in _selectableEntities
                where selectableEntity.Entity.OwnerPlayerID == _localPlayer.ID
                select selectableEntity;
            
            HighlightEntities(playerEntities.Any() ? playerEntities : _selectableEntities);
        }

        private void HighlightEntities(IEnumerable<ISelectableEntity> selectableEntities)
        {
            ClearHighlighted();

            foreach (var selectableEntity in selectableEntities)
            {
                if (_highlightedEntities.Add(selectableEntity))
                {
                    _highlightHandler.Enable(selectableEntity.Entity, _localPlayer.GetRelationsFor(selectableEntity.Entity.OwnerPlayerID));
                }
            }
        }

        private void ClearHighlighted()
        {
            if (_highlightedEntities?.Count > 0)
            {
                foreach (var selectableEntity in _highlightedEntities)
                {
                    _highlightHandler.Disable(selectableEntity.Entity);
                }

                _highlightedEntities.Clear();
            }
        }
    }
}