using System.Collections.Generic;
using Code.EntityModule;
using Code.Installers;
using Code.PlayerModule;
using Code.Queries;
using Code.Selection.Abstract;
using Code.Selection.Commands;
using Code.Selection.Events;
using Code.Selection.Strategies;
using Code.Services.CommandBus;
using Code.Services.EventService;
using Code.Services.QueryBus;
using UnityEngine;
using Zenject;

namespace Code.Selection
{
    /// <summary>
    /// Base selection handler class.
    /// </summary>
    public class SelectionVisitor : ISelectionVisitor,
        IEventHandler<IOnDragSelectionEnter>,
        IEventHandler<IOnDragSelectionExit>,
        IEventHandler<IOnDragSelectionBegin>,
        IEventHandler<IOnDragSelectionEnd>
    {
        private class OnSelection : IOnSelectedEntity, IOnDeselectedEntity
        {
            public IEntity SelectedEntity { get; set; }
            public IPlayer SelectedPlayer { get; set; }
        }

        private class OnSelectionEvent<T> : IOnSelected<T>, IOnDeselected<T>
        {
            public T Selectable { get; set; }
            public IPlayer Player { get; set; }
        }

        private class ClearDragSelectionCommand : IClearBoxSelection
        {
            public void Execute(ISelectionDragHandler payload)
            {
                payload.ClearAll();
            }
        }

        private readonly ComponentFromSceneQuery<ISelectableEntity> _selectableQuery;
        private readonly ISelectionGroupFilterStrategy _selectionGroupStrategy;
        private readonly OnSelectionEvent<Building> _buildingSelectionEvent;
        private readonly OnSelectionEvent<Item> _itemSelectionEvent;
        private readonly OnSelectionEvent<Unit> _unitSelectionEvent;
        private readonly List<ISelectableEntity> _groupSelection;
        private readonly IClearBoxSelection _clearDragSelection;
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly OnSelection _onSelectionEntityEvent;
        private readonly IQueryResolver _queryResolver;
        private readonly IEventInvoker _eventInvoker;
        private readonly List<Unit> _selectedUnits;
        private readonly IPlayer _player;
        private ISelectableEntity _singleSelection;
        private Vector3 _startDragSelection;
        private Building _selectedBuilding;
        private Item _selectedItem;

        public SelectionVisitor(
            [InjectOptional(Id = BindID.LocalPlayer)]
            IPlayer player,
            ICommandDispatcher commandDispatcher,
            IQueryResolver queryResolver,
            IEventInvoker eventInvoker)
        {
            _player = player;
            _commandDispatcher = commandDispatcher;
            _queryResolver = queryResolver;
            _eventInvoker = eventInvoker;
            _clearDragSelection = new ClearDragSelectionCommand();
            _selectedUnits = new List<Unit>();
            _buildingSelectionEvent = new OnSelectionEvent<Building>();
            _itemSelectionEvent = new OnSelectionEvent<Item>();
            _unitSelectionEvent = new OnSelectionEvent<Unit>();
            _selectableQuery = new ComponentFromSceneQuery<ISelectableEntity>();
            _selectionGroupStrategy = new SelectionGroupStrategy();
            _groupSelection = new List<ISelectableEntity>();
            _onSelectionEntityEvent = new OnSelection();
        }

        // Handling box group selection
        public void Handle(IOnDragSelectionEnter triggeredEvent)
        {
            if (triggeredEvent.Selectable != null) _groupSelection.Add(triggeredEvent.Selectable);
        }

        // Handling box group selection
        public void Handle(IOnDragSelectionExit triggeredEvent)
        {
            if (triggeredEvent.Selectable != null) _groupSelection.Remove(triggeredEvent.Selectable);
        }

        public void Handle(IOnDragSelectionBegin triggeredEvent)
        {
            _selectableQuery.ScreenPoint = triggeredEvent.PointerPosition;
            _singleSelection = _queryResolver.Resolve<IComponentFromSceneQuery<ISelectableEntity>, ISelectableEntity>(_selectableQuery);
        }

        public void Handle(IOnDragSelectionEnd triggeredEvent)
        {
            if (_groupSelection?.Count > 0)
            {
                _selectionGroupStrategy.Filter(_player, _groupSelection, this);
            }
            else
            {
                if (_singleSelection != null)
                {
                    _selectableQuery.ScreenPoint = triggeredEvent.PointerPosition;
                    var selectable = _queryResolver.Resolve<IComponentFromSceneQuery<ISelectableEntity>, ISelectableEntity>(_selectableQuery);
                    
                    if (_singleSelection == selectable)
                    {
                        ClearSelectionFor(_player);
                        _singleSelection?.VisitSelection(this);
                    }
                }
            }

            _commandDispatcher.Dispatch(_clearDragSelection);
        }
        
        // Select unit group
        public void Select(IEnumerable<Unit> unitsGroup)
        {
            if (unitsGroup != null)
            {
                ClearSelectionFor(_player);
                foreach (var unit in unitsGroup)
                {
                    _selectedUnits.Add(unit);
                    InvokeSelectionEvent(unit, _player);
                }
            }
        }

        public void Select(Unit unit)
        {
            if (unit == null) return;

            if (_selectedUnits.Contains(unit)) return;

            ClearSelectionFor(_player);
            _selectedUnits.Add(unit);
            InvokeSelectionEvent(unit, _player);
        }

        public void Select(Item item)
        {
            if (item == null) return;
            ClearSelectionFor(_player);
            _selectedItem = item;
            InvokeSelectionEvent(item, _player);
        }

        public void Select(Building building)
        {
            if (IsNullParams(building, _player)) return;
            ClearSelectionFor(_player);
            _selectedBuilding = building;
            InvokeSelectionEvent(building, _player);
        }

        public void DeselectFor(Unit unit, IPlayer player)
        {
            if (IsNullParams(unit, player)) return;
            if (!player.IsLocal) return;
            if (_selectedUnits.Remove(unit)) InvokeDeselectEvent(unit, player);
        }

        public void DeselectFor(Item item, IPlayer player)
        {
            if (IsNullParams(item, player)) return;
            if (!player.IsLocal) return;
            if (item != _selectedItem) return;

            _selectedItem = null;
            InvokeDeselectEvent(item, player);
        }

        public void DeselectFor(Building building, IPlayer player)
        {
            if (IsNullParams(building, player)) return;
            if (!player.IsLocal) return;
            if (building != _selectedBuilding) return;

            _selectedBuilding = null;
            InvokeDeselectEvent(building, player);
        }

        private void ClearSelectionFor(IPlayer player)
        {
            if (player == null) return;
            if (!player.IsLocal) return;

            ClearUnitsSelectionFor(player);
            ClearItemSelectionFor(player);
            ClearBuildingSelectionFor(player);
        }

        private void ClearBuildingSelectionFor(IPlayer player)
        {
            if (player == null) return;
            if (!player.IsLocal) return;
            if (_selectedBuilding) DeselectFor(_selectedBuilding, player);
        }

        private void ClearItemSelectionFor(IPlayer player)
        {
            if (player == null) return;
            if (!player.IsLocal) return;
            if (_selectedItem) DeselectFor(_selectedItem, player);
        }

        private void ClearUnitsSelectionFor(IPlayer player)
        {
            if (player == null) return;
            if (!player.IsLocal) return;
            if (_selectedUnits == null) return;

            foreach (var unit in _selectedUnits.ToArray())
            {
                DeselectFor(unit, player);
            }
        }

        private static bool IsNullParams(object lhr, object rhr)
        {
            return lhr == null || rhr == null;
        }

        private void InvokeSelectionEvent(Unit unit, IPlayer player)
        {
            _unitSelectionEvent.Selectable = unit;
            _unitSelectionEvent.Player = player;
            InvokeSelectEntity(unit);
            _eventInvoker.Invoke<IOnSelected<Unit>>(_unitSelectionEvent);
            LogSelection(_unitSelectionEvent.Selectable.name);
        }

        private void InvokeSelectionEvent(Item item, IPlayer player)
        {
            _itemSelectionEvent.Selectable = item;
            _itemSelectionEvent.Player = player;
            InvokeSelectEntity(item);
            _eventInvoker.Invoke<IOnSelected<Item>>(_itemSelectionEvent);
            LogSelection(_itemSelectionEvent.Selectable.name);
        }

        private void InvokeSelectionEvent(Building building, IPlayer player)
        {
            _buildingSelectionEvent.Selectable = building;
            _buildingSelectionEvent.Player = player;
            InvokeSelectEntity(building);
            _eventInvoker.Invoke<IOnSelected<Building>>(_buildingSelectionEvent);
            LogSelection(_buildingSelectionEvent.Selectable.name);
        }

        private void InvokeDeselectEvent(Unit unit, IPlayer player)
        {
            _unitSelectionEvent.Player = player;
            _unitSelectionEvent.Selectable = unit;
            InvokeDeselectEntity(unit);
            _eventInvoker.Invoke<IOnDeselected<Unit>>(_unitSelectionEvent);
            LogDeselection($"{unit.name}");
        }

        private void InvokeDeselectEvent(Item item, IPlayer player)
        {
            _itemSelectionEvent.Player = player;
            _itemSelectionEvent.Selectable = item;
            InvokeDeselectEntity(item);
            _eventInvoker.Invoke<IOnDeselected<Item>>(_itemSelectionEvent);
            LogDeselection($"{item.name}");
        }

        private void InvokeDeselectEvent(Building building, IPlayer player)
        {
            _buildingSelectionEvent.Player = player;
            _buildingSelectionEvent.Selectable = building;
            InvokeDeselectEntity(building);
            _eventInvoker.Invoke<IOnDeselected<Building>>(_buildingSelectionEvent);
            LogDeselection($"{building.name}");
        }

        private void InvokeSelectEntity(IEntity entity)
        {
            _onSelectionEntityEvent.SelectedEntity = entity;
            _onSelectionEntityEvent.SelectedPlayer = _player;
            _eventInvoker.Invoke<IOnSelectedEntity>(_onSelectionEntityEvent);
        }

        private void InvokeDeselectEntity(IEntity entity)
        {
            _onSelectionEntityEvent.SelectedEntity = entity;
            _onSelectionEntityEvent.SelectedPlayer = _player;
            _eventInvoker.Invoke<IOnDeselectedEntity>(_onSelectionEntityEvent);
        }

        private static void LogSelection(string message)
        {
            Log($"{nameof(Select)}: {message}");
        }

        private static void LogDeselection(string message)
        {
            Log($"{nameof(DeselectFor)}: {message}");
        }

        private static void Log(string message)
        {
            Debug.Log($"{nameof(SelectionVisitor)}.{message}");
        }
    }
}