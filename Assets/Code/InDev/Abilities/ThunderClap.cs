using Code.EntityModule;
using Code.InDev.Orders.Abstract;
using Code.Selection.Events;
using Code.Services.EventService;
using UnityEngine;

namespace Code.InDev.Abilities
{
    public interface IThunderClapOrder : IOrder<Unit>
    {
        
    }
    
    [CreateAssetMenu(menuName = "Ability/Create ThunderClap", fileName = "ThunderClap", order = 0)]
    public sealed class ThunderClap : Ability<Unit>, IOrderHandler<IThunderClapOrder>, IEventHandler<IOnDragSelectionBegin>
    {
        private class ThunderClapOrder : IThunderClapOrder
        {
            public ThunderClapOrder(Unit caster) => Caster = caster;

            public Unit Caster { get; }
        }

        protected override void IssueOrderCaster()
        {
            _caster.IssueOrder<IThunderClapOrder>(new ThunderClapOrder(_caster));
        }

        public bool ExecuteOrder(IThunderClapOrder order)
        {
            Debug.Log("Thunder Clap Cast");
            return true;
        }

        public void Handle(IOnDragSelectionBegin triggeredEvent)
        {
            
        }
    }
}