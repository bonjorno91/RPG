using Code.Behaviours;
using Code.GameModule;
using Code.PlayerModule;
using Code.Queries;
using Code.Selection;
using Code.Services;
using Code.Services.CommandBus;
using Code.Services.EventService;
using Code.Services.QueryBus;
using Code.UI;
using UnityEngine;
using Zenject;

namespace Code.Installers
{
    public class ServicesInstaller : MonoInstaller
    {
        [SerializeField] private CameraConfig _config;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Transform _playerTransform;

        [Header("Selection Marks")] 
        [SerializeField] private SelectionMarkConfig _selectionMarkConfig;
    
        [Header("UI")] 
        [SerializeField] private GameObject _uiUnitPanel;
        [SerializeField] private Transform _uiRoot;
        [SerializeField] private GameObject _guiDragHandler;

        public override void InstallBindings()
        {
            // Bind --- UI
            Container.Bind<SelectionMarkConfig>().FromNewScriptableObject(_selectionMarkConfig).AsSingle();
            Container.Bind<SelectionMarksPool>().AsSingle();
        
            // Bind --- Players
            var humanPlayer = new Player("Human", PlayerID.Player1, isAI: false, isLocal: true);
            var aiPlayer = new Player("AI", PlayerID.Player2, isAI: true, isLocal: false);
            Container.Bind<IPlayer>().WithId(PlayerController.Human).FromInstance(humanPlayer).AsCached();
            Container.Bind<IPlayer>().WithId(BindID.LocalPlayer).FromInstance(humanPlayer).AsCached();
            Container.Bind<IPlayer>().WithId(PlayerController.AI).FromInstance(humanPlayer).AsCached();

            // Bind --- Bus Services
            Container.BindInterfacesTo<EventBusService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<QueryBusService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<CommandBusService>().AsSingle().NonLazy();
            Container.Bind<EntityEventBusInjector>().AsSingle().NonLazy();

            // Bind --- Handlers
            Container.Bind<SelectionHandler>().AsCached();
            Container.Bind<IEntityPanel>().FromComponentInNewPrefab(_uiUnitPanel).UnderTransform(_uiRoot).AsSingle()
                .WhenInjectedInto<SelectionUIHandler>();
            Container.Bind<SelectionVisitor>().AsCached();
            Container.Bind<SelectionUIHandler>().AsCached();
            Container.Bind<SelectionDragHandler>().AsCached();
            Container.Bind<SelectionMarkHandler>().AsCached();
            Container.Bind<SelectionDragUIHandler>().FromComponentInNewPrefab(_guiDragHandler).AsSingle();
            Container.Bind<SelectionHighlightHandler>().AsCached();

            // Container.Bind<GameObject>().FromInstance(Instantiate(_highlightEffect)).AsTransient().WhenInjectedInto<PointerOverHighlightableHandler>();
            // Container.Bind<HUDSelection>().FromComponentInNewPrefab(_selectionMarkGreen).AsTransient()
            // .WhenInjectedInto<SelectionSingleHandler>();
            // Container.Bind<HUDSelection>().FromComponentInNewPrefab(_selectionMarkYellow).AsTransient()
            // .WhenInjectedInto<SelectionHighlightHandler>();
            Container.Bind<Camera>().FromInstance(_mainCamera).AsSingle();
            Container.BindInterfacesTo<PointerPositionQuery>().AsSingle();
            Container.BindInterfacesTo<SceneRaycastQueryHandler>().AsSingle().NonLazy();
            Container.BindInterfacesTo<PointerService>().AsCached();
            Container.BindInterfacesTo<Game>().AsSingle().NonLazy();

            Container.Bind<CameraConfig>().FromInstance(_config).WhenInjectedInto<FollowCameraService>();
            Container.Bind<Transform>().WithId(BindID.PlayerTransform).FromInstance(_playerTransform).AsSingle();
            Container.BindInterfacesTo<FollowCameraService>().AsSingle();
        }
    }

    public enum PlayerController
    {
        Human,
        AI
    }

    public enum BindID
    {
        PlayerTransform,
        LocalPlayer
    }
}