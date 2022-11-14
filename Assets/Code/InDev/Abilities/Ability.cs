using System;
using System.Collections.Generic;
using Code.EntityModule;
using Code.InDev.Abilities.Validation.Abstract;
using Code.InDev.Orders;
using Code.InDev.Orders.Abstract;
using Code.Services.EventService;
using Code.UI;
using UnityEngine;
using UnityEngine.AI;

namespace Code.InDev.Abilities
{
    public interface ITooltipDrawer
    {
        void DrawTooltip(ITooltip tooltip);
    }

    public interface IAbility<TCaster> where TCaster : IEntity
    {
        ICasterValidator<TCaster, IAbility<TCaster>> CasterValidator { get; }
    }
    
    public abstract class Ability<TCaster> : ScriptableObject, IAbility<TCaster>, IOrderHandler<IMoveOrder<Vector3>>, IOrderHandler<ISmartOrder<Vector3>>, ITooltipDrawer
    where TCaster : IEntity
    {
        private class Order : IMoveOrder<Vector3>
        {
            public Unit Caster { get; set; }
            public Vector3 Target { get; set; }
        }
        
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Name { get; private set; } = "New Ability";
        [field: Multiline]
        [field: SerializeField] public string Description { get; private set; } = "New Ability Description";
        [field: SerializeField] public KeyCode Hotkey { get; private set; } = KeyCode.A;
        [field: SerializeField] public float Cooldown { get; private set; } = 0;
        [field: SerializeField] private AbilityButtonCommand _rightClickCommand;
        [field: SerializeField] private AbilityButtonCommand _leftClickCommand;
        [field: SerializeField] private List<Resource> _resources = new();
        [field: SerializeField] private List<Stat> _stats = new();
        [field: SerializeField] private AbilityCasterValidator<TCaster, IAbility<TCaster>> _casterValidator;

        public ICasterValidator<TCaster, IAbility<TCaster>> CasterValidator => _casterValidator;
        protected IEventInvoker _eventInvoker;
        protected TCaster _caster;
        private NavMeshAgent _agent;
        protected abstract void IssueOrderCaster();

        public void DrawButton(IButtonsPanel buttonsPanel)
        {
            if (Icon)
            {
                var button = buttonsPanel.GetButton(Icon);
                button.SetHotkey(Hotkey);
                button.SetTooltipDrawer(this);
                if(_resources?.Count > 0) 
                    button.SetCost(_resources[0].Value);
                
                button.SetLeftClickCommand(OnLeftClick);
                button.SetRightClickCommand(OnRightClick);
            }
        }

        private void OnRightClick(IButtonsPanel buttonsPanel)
        {
            
        }

        private void OnLeftClick(IButtonsPanel buttonsPanel)
        {
            var errorMessage = string.Empty;
            
            if (_casterValidator.IsValid(_caster, this,ref errorMessage))
            {
                IssueOrderCaster();
            }
            else
            {
                Debug.LogWarning(errorMessage);
            }
        }

        public void DrawStats(IStatsPanel statsPanel)
        {
            foreach (var stat in _stats)
            {
                statsPanel.GetStatDrawer(stat);
            }
        }

        public void DrawTooltip(ITooltip tooltip)
        {
            tooltip.SetName(Name);

            if (_resources?.Count != 0)
            {
                foreach (var resource in _resources)
                {
                    tooltip.AddResource(resource);
                }
            }

            if(Cooldown != 0) tooltip.SetCooldown($"{Cooldown.ToString()} sec.");
            
            tooltip.SetDescription(Description);
        }

        public void OnInitialize(TCaster caster,IEventInvoker eventInvoker)
        {
            _caster = caster;
            _eventInvoker = eventInvoker;
            _agent = caster.Origin.gameObject.AddComponent<NavMeshAgent>();
        }

        public void OnRemove(Unit unit)
        {
            Destroy(_agent);
        }

        public void Update(Unit unit)
        {
            
        }

        public bool ExecuteOrder(ISmartOrder<Vector3> order)
        {
            if (_agent)
            {
                _agent.destination = order.Target;
                Debug.Log($"ORDER: SMART MOVE {order.Caster} TO {order.Target.ToString()}");
                return true;
            }

            return false;
        }

        public bool ExecuteOrder(IMoveOrder<Vector3> order)
        {
            Debug.Log($"ORDER: MOVE {order.Caster} TO {order.Target.ToString()}");
            return true;
        }
    }


    public interface ITargetAbility<TTarget>
    {
        ITargetValidator<TTarget,ITargetAbility<TTarget>> TargetValidator { get; }
    }

    [Serializable]
    public class Stat
    {
        [field: SerializeField] public StatType StatType { get; private set; }
        [field: SerializeField] public int Value { get; private set; }
    }

    [Serializable]
    public class Resource
    {
        [field: SerializeField] public ResourceType ResourceType { get; private set; }
        [field: SerializeField] public int Value { get; private set; }
    }


}