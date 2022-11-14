using System;
using System.Collections.Generic;
using Code.InDev.Orders.Abstract;
using Code.PlayerModule;
using Code.Services.EventService;
using Code.UI;
using UnityEngine;

namespace Code.EntityModule
{
    public abstract class Entity : MonoBehaviour, IEntity
    {
        public static event Action<Action<IEventBus>> OnNewEntityInject;
        private static readonly HashSet<Entity> EntitiesHashSet = new();

        [field: SerializeField] public bool IsDead { get; private set; }
        [field: SerializeField] public bool IsPaused { get; private set; }
        [field: SerializeField] public PlayerID OwnerPlayerID { get; private set; }
        [field: SerializeField] public Transform Origin { get; private set; }
        public abstract EntityTags Tags { get; set; }
        
        protected readonly Dictionary<Type, List<object>> _orderHandlers = new();
        protected IEventBus _eventBus { get; private set; }
        
        private void InjectEventBus(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        
        private void Initialize()
        {
            Origin = gameObject.transform;
        }

        protected void Awake()
        {
            if (EntitiesHashSet.Add(this)) OnNewEntityInject?.Invoke(InjectEventBus);
            if (!Origin) Initialize();
        }

        private void Reset()
        {
            Initialize();
        }
        
        public void IssueOrder<TOrder>(TOrder order) where TOrder : IOrder
        {
            if (_orderHandlers.TryGetValue(typeof(IOrderHandler<TOrder>), out var handlers))
                foreach (var handler in handlers)
                    if (handler is IOrderHandler<TOrder> orderHandler)
                        if (orderHandler.ExecuteOrder(order))
                            return;
        }

        public abstract void DrawPanelFor(IEntityPanelProvider entityPanel, IPlayer selectedPlayer);
    }
}