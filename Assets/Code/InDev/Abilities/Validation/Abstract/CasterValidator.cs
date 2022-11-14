using System;
using Code.EntityModule;
using UnityEngine;

namespace Code.InDev.Abilities.Validation.Abstract
{
    [Serializable]
    public abstract class CasterValidator<TObject, TAbility> : ScriptableObject, ICasterValidator<TObject, TAbility> where TAbility : IAbility<TObject> where TObject : IEntity
    {
        public abstract bool IsValid(TObject validationObject, TAbility ability, ref string errorMessage);
    }
}