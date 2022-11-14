using Code.EntityModule;
using Code.PlayerModule;
using UnityEngine;

namespace Code.UI
{
    public interface IEntityPanelProvider
    {
        IInfoPanel InfoPanel { get; }
        IButtonsPanel ButtonsPanel { get; }
        IStatsPanel StatsPanel { get; }
    }

    public interface IEntityPanel : IEntityPanelProvider
    {
        void Show(IEntity entity, IPlayer player);
        void Hide(IEntity entity);
    }

    public class EntityPanel : MonoBehaviour, IEntityPanel
    {
        [SerializeField] private ButtonsPanel _buttonsPanel;
        [SerializeField] private InfoPanel _infoPanel;
        [SerializeField] private StatsPanel _statsPanel;
        private bool _useButtonPanel;
        private bool _useStatsPanel;

        private void Awake()
        {
            _buttonsPanel.Initialize();
            _infoPanel.Initialize();
            _statsPanel.Initialize();
            gameObject.SetActive(false);
        }

        public IButtonsPanel ButtonsPanel
        {
            get
            {
                _useButtonPanel = true;
                return _buttonsPanel;
            }
        }

        public IStatsPanel StatsPanel
        {
            get
            {
                _useStatsPanel = true;
                return _statsPanel;
            }
        }

        public IInfoPanel InfoPanel => _infoPanel;

        public void Show(IEntity entity, IPlayer player)
        {
            _buttonsPanel.ClearPanel();
            _statsPanel.ClearPanel();
            entity.DrawPanelFor(this, player);
            _buttonsPanel.gameObject.SetActive(_useButtonPanel);
            _statsPanel.gameObject.SetActive(_useStatsPanel);
            gameObject.SetActive(true);
        }

        public void Hide(IEntity entity)
        {
            gameObject.SetActive(false);
            _useButtonPanel = false;
            _useStatsPanel = false;
        }
    }
}