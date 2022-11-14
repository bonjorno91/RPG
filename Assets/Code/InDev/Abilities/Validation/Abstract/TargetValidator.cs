using System;
using UnityEngine;

namespace Code.InDev.Abilities.Validation.Abstract
{
    [Serializable]
    public abstract class TargetValidator<TTarget, TAbility> : ScriptableObject, ITargetValidator<TTarget, TAbility> where TAbility : ITargetAbility<TTarget>
    {
        public abstract bool IsValid(TTarget validationObject, TAbility ability,ref string errorMessage);
    }
}