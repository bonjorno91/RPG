using System.Collections.Generic;
using Code.Installers;
using Code.PlayerModule;
using Code.Queries;
using Code.Selection;
using Code.Selection.Abstract;
using Code.Selection.Events;
using Code.Selection.Strategies;
using Code.Services.CommandBus;
using Code.Services.EventService;
using Code.Services.QueryBus;
using UnityEngine;
using Zenject;


namespace Code.GameModule
{
    public class Game : ITickable
    {
        private readonly IQueryHandler<ISceneRaycastQuery,RaycastHit> _raycastQueryHandler;
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly IEventBus _eventBus;
        private readonly PlayerInput _playerInput;
        private readonly SelectionVisitor _selectionVisitor;
        private readonly List<IPlayerController> _playerControllers = new();

        public Game(
            [InjectOptional(Id = PlayerController.Human)]
            IPlayer humanPlayer,
            [InjectOptional(Id = PlayerController.AI)]
            IPlayer aiPlayer,
            Camera camera,
            IEventBus eventBus,
            IQueryBus queryBus,
            ICommandBus commandBus,
            SelectionVisitor selectionVisitor,
            SelectionUIHandler selectionUIHandler,
            SelectionDragHandler selectionDragHandler,
            SelectionMarkHandler selectionMarkHandler,
            SelectionDragUIHandler selectionDragUIHandler,
            SelectionHighlightHandler selectionHighlightHandler,
            SelectionHandler selectionHandler)
        {
            _eventBus = eventBus;
            _queryBus = queryBus;
            _commandBus = commandBus;
            
            var humanPlayerInputStrategy = new SelectStrategy(_eventBus);
            var humanPlayerInput = new PlayerInput(humanPlayerInputStrategy);
            var humanPlayerController = new HumanPlayerController(humanPlayer, humanPlayerInput);
            var aiPlayerController = new AIPlayerController(aiPlayer);

            _playerControllers.Add(humanPlayerController);
            _playerControllers.Add(aiPlayerController);
            
            // Bind --- Get component from scene by raycast query handler
            _queryBus.Bind(new ComponentFromSceneQueryHandler<ISelectableEntity>(camera));

            // Subscribe --- GUI Drag Selection Box Event Handler
            _eventBus.Subscribe<IOnDragSelectionBegin>(selectionDragUIHandler);
            _eventBus.Subscribe<IOnDragSelectionHandled>(selectionDragUIHandler);
            _eventBus.Subscribe<IOnDragSelectionEnd>(selectionDragUIHandler);
            
            // Subscribe --- Drag Selection Box Event Handler
            _eventBus.Subscribe<IOnDragSelectionBegin>(selectionDragHandler);
            _eventBus.Subscribe<IOnDragSelectionHandled>(selectionDragHandler);
            
            // Bind --- Clear Drag Selection Command Handler
            _commandBus.Bind(selectionDragHandler);
            
            // Subscribe --- Highlight Selection Event Handler
            _eventBus.Subscribe<IOnDragSelectionEnter>(selectionHighlightHandler);
            _eventBus.Subscribe<IOnDragSelectionExit>(selectionHighlightHandler);
            
            // Subscribe --- Selection handler
            _eventBus.Subscribe<IOnDragSelectionBegin>(selectionVisitor);
            _eventBus.Subscribe<IOnDragSelectionEnd>(selectionVisitor);
            _eventBus.Subscribe<IOnDragSelectionEnter>(selectionVisitor);
            _eventBus.Subscribe<IOnDragSelectionExit>(selectionVisitor);
            _eventBus.Subscribe<IOnSelectedEntity>(selectionMarkHandler);
            _eventBus.Subscribe<IOnDeselectedEntity>(selectionMarkHandler);
            
            _eventBus.Subscribe<IOnSelectedEntity>(selectionHandler);
            _eventBus.Subscribe<IOnDeselectedEntity>(selectionHandler);
            _eventBus.Subscribe<IOnRightClickOrder>(selectionHandler);
            
            // Subscribe --- UI Handlers
            _eventBus.Subscribe<IOnSelectedEntity>(selectionUIHandler);
            _eventBus.Subscribe<IOnDeselectedEntity>(selectionUIHandler);
        }

        public void Tick()
        {
            foreach (var playerController in _playerControllers)
            {
                playerController.Update();
            }
        }
    }
}