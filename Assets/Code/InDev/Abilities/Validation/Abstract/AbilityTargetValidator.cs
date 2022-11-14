using System;
using System.Collections.Generic;
using Code.EntityModule;
using UnityEngine;

namespace Code.InDev.Abilities.Validation.Abstract
{
    [Serializable]
    public sealed class AbilityTargetValidator<TTarget, TAbility> : ITargetValidator<TTarget,TAbility> 
        where TAbility : ITargetAbility<TTarget>
    where TTarget : IEntity
    {
        [SerializeField] private EntityTags _includeTags;
        [SerializeField] private EntityTags _excludeTags;
        [SerializeField] private List<TargetValidator<TTarget, TAbility>> _validators;

        public bool IsValid(TTarget target, TAbility ability, ref string errorMessage)
        {
            var includeTag = target.Tags & _includeTags;

            if (includeTag != _includeTags)
            {
                errorMessage = includeTag.ToString();
                return false;
            }

            var excludeTag = target.Tags & _excludeTags;
            
            if (excludeTag != 0)
            {
                errorMessage = excludeTag.ToString();
                return false;
            }

            if (_validators is {Count: > 0})
            {
                foreach (var validator in _validators)
                {
                    if (!validator.IsValid(target, ability, ref errorMessage)) return false;
                }
            }

            return true;

            return true;
        }
    }
}