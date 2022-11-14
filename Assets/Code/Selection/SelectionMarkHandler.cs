using System;
using System.Collections.Generic;
using Code.Behaviours;
using Code.EntityModule;
using Code.Installers;
using Code.PlayerModule;
using Code.Selection.Events;
using Code.Services.EventService;
using Zenject;

namespace Code.Selection
{
    public class SelectionMarkHandler : IEventHandler<IOnSelectedEntity>, IEventHandler<IOnDeselectedEntity>
    {
        private readonly Dictionary<IEntity,SelectionCircle> _selections;
        private readonly SelectionMarksPool _marksPool;
        private readonly IPlayer _localPlayer;

        public SelectionMarkHandler([Inject(Id = BindID.LocalPlayer)]IPlayer localPlayer, SelectionMarksPool marksPool)
        {
            _selections = new Dictionary<IEntity, SelectionCircle>();
            _marksPool = marksPool;
            _localPlayer = localPlayer;
        }
        
        public void Handle(IOnSelectedEntity triggeredEvent)
        {
            if (_localPlayer == triggeredEvent.SelectedPlayer)
            {
                var relation = _localPlayer.GetRelationsFor(triggeredEvent.SelectedEntity.OwnerPlayerID);
                var mark  = _marksPool.Get(relation);
                mark.Select(triggeredEvent.SelectedEntity.Origin);
                _selections.Add(triggeredEvent.SelectedEntity, mark);
            }
        }

        public void Handle(IOnDeselectedEntity triggeredEvent)
        {
            if (_localPlayer == triggeredEvent.SelectedPlayer)
            {
                if (_selections.Remove(triggeredEvent.SelectedEntity, out var mark))
                {
                    var relation = _localPlayer.GetRelationsFor(triggeredEvent.SelectedEntity.OwnerPlayerID);
                    mark.Select(triggeredEvent.SelectedEntity.Origin);
                    _marksPool.Release(mark);
                }
            }
        }
    }
}