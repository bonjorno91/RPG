using System.Collections.Generic;
using Code.EntityModule;
using Code.InDev.Orders;
using Code.Installers;
using Code.PlayerModule;
using Code.Selection.Events;
using Code.Services.EventService;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Code.Selection
{
    public class SelectionHandler : IEventHandler<IOnSelectedEntity>,IEventHandler<IOnDeselectedEntity>,IEventHandler<IOnRightClickOrder>
    {
        private readonly IPlayer _player;
        private readonly HashSet<IEntity> _selectedGroup = new();

        public SelectionHandler([Inject(Id = BindID.LocalPlayer)]IPlayer player)
        {
            _player = player;
        }

        public void Handle(IOnSelectedEntity triggeredEvent)
        {
            if (triggeredEvent.SelectedPlayer == _player)
            {
                _selectedGroup.Add(triggeredEvent.SelectedEntity);
            }
        }

        public void Handle(IOnDeselectedEntity triggeredEvent)
        {
            _selectedGroup.Remove(triggeredEvent.SelectedEntity);
        }

        public void Handle(IOnRightClickOrder triggeredEvent)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out var result))
            {
                foreach (var entity in _selectedGroup)
                {
                    entity.IssueOrder<ISmartOrder<Vector3>>(new SmartOrder<Vector3>(){Caster = entity,Target = result.point});
                }
            }
        }
        
        private class SmartOrder<TTarget> : ISmartOrder<TTarget>
        {
            public IEntity Caster { get; set; }
            public TTarget Target { get; set; }
        }
    }
}