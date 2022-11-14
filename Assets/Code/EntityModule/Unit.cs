using System.Collections.Generic;
using Code.ActionsModule.Abilities;
using Code.InDev.Abilities;
using Code.InDev.Orders.Abstract;
using Code.PlayerModule;
using Code.Services.EventService;
using Code.UI;
using UnityEngine;

namespace Code.EntityModule
{
    [SelectionBase]
    public class Unit : Entity
    {
        [field: SerializeField] public override EntityTags Tags { get; set; } = EntityTags.Unit;
        [SerializeField] private List<Ability<Unit>> _abilitiesList;
        private readonly List<Ability<Unit>> _abilities = new();


        private void Start()
        {
            foreach (var ability in _abilitiesList)
            {
                var abilityInstance = Instantiate(ability);
                var handlers = ability.GetType().GetInterfaces();
                abilityInstance.OnInitialize(this, _eventBus);
                _abilities.Add(abilityInstance);
                
                foreach (var handler in handlers)
                {
                    if (handler.IsGenericType && handler.GetGenericTypeDefinition() == typeof(IOrderHandler<>))
                    {
                        if (!_orderHandlers.ContainsKey(handler)) _orderHandlers.Add(handler, new List<object>());
                        
                        _orderHandlers[handler].Add(abilityInstance);
                    }
                }
                
                SubscribeAbilityOnEvents(ability);
            }
        }

        private void Update()
        {
            foreach (var ability in _abilities)
            {
                ability.Update(this);
            }
        }

        public void AddAbility(Ability<Unit> ability)
        {
            if (_abilities.Contains(ability)) return;

            _abilities.Add(ability);
            ability.OnInitialize(this, _eventBus);
            SubscribeAbilityOnEvents(ability);
        }

        public void RemoveAbility(Ability<Unit> ability)
        {
            if (_abilities.Remove(ability))
            {
                ability.OnRemove(this);
            }
        }

        public override void DrawPanelFor(IEntityPanelProvider entityPanel, IPlayer selectedPlayer)
        {
            entityPanel.InfoPanel.EntityName = gameObject.name;
            
            foreach (var ability in _abilities)
            {
                ability.DrawButton(entityPanel.ButtonsPanel);
                ability.DrawStats(entityPanel.StatsPanel);
            }
        }

        public void SubscribeAbilityOnEvents(Ability<Unit> ability)
        {
            var interfaces = ability.GetType().GetInterfaces();

            if (interfaces?.Length > 0)
            {
                foreach (var type in interfaces)
                {
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEventHandler<>))
                    {
                        var eventTypes = type.GetGenericArguments();

                        foreach (var eventType in eventTypes)
                        {
                            _eventBus.Subscribe(eventType,ability);
                        }
                    }
                }
            }
        }

        public float GetStat(StatType statType)
        {
            return 50;
        }
    }
}