using Code.EntityModule;
using Code.InDev.Abilities.Validation.Abstract;
using UnityEngine;

namespace Code.InDev.Abilities
{
    public class EntityTargetAbility<TCaster,TTarget> : Ability<TCaster>, ITargetAbility<TTarget> where TCaster : IEntity where TTarget : IEntity
    {
        public ITargetValidator<TTarget, ITargetAbility<TTarget>> TargetValidator => _targetValidator;
        [field: SerializeField] private AbilityTargetValidator<TTarget, ITargetAbility<TTarget>> _targetValidator;
        protected override void IssueOrderCaster()
        {
            
        }
    }
}