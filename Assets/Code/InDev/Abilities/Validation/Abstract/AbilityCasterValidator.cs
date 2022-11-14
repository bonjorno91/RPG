using System;
using System.Collections.Generic;
using Code.EntityModule;
using UnityEngine;

namespace Code.InDev.Abilities.Validation.Abstract
{
    [Serializable]
    public sealed class AbilityCasterValidator<TCaster, TAbility> : ICasterValidator<TCaster, TAbility>
        where TAbility : IAbility<TCaster>
        where TCaster : IEntity
    {
        [SerializeField] private EntityTags _includeTags;
        [SerializeField] private EntityTags _excludeTags;
        [SerializeField] private List<CasterValidator<TCaster, TAbility>> _validators;

        public bool IsValid(TCaster caster, TAbility ability, ref string errorMessage)
        {
            var includeTag = caster.Tags & _includeTags;

            if (includeTag != _includeTags)
            {
                errorMessage = includeTag.ToString();
                return false;
            }

            var excludeTag = caster.Tags & _excludeTags;
            
            if (excludeTag != 0)
            {
                errorMessage = excludeTag.ToString();
                return false;
            }

            if (_validators is {Count: > 0})
            {
                foreach (var validator in _validators)
                {
                    if (!validator.IsValid(caster, ability, ref errorMessage)) return false;
                }
            }

            return true;
        }
    }
}