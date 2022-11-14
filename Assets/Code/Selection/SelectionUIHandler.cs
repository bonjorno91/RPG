using Code.Selection.Events;
using Code.Services.EventService;
using Code.UI;

namespace Code.Selection
{
    public class SelectionUIHandler :
        IEventHandler<IOnSelectedEntity>,
        IEventHandler<IOnDeselectedEntity>
    {
        private readonly IEntityPanel _entityPanel;

        public SelectionUIHandler(IEntityPanel entityPanel) => _entityPanel = entityPanel;

        public void Handle(IOnSelectedEntity triggeredEvent)
        {
            _entityPanel.Show(triggeredEvent.SelectedEntity, triggeredEvent.SelectedPlayer);
        }

        public void Handle(IOnDeselectedEntity triggeredEvent)
        {
            _entityPanel.Hide(triggeredEvent.SelectedEntity);
        }
    }
}