using Code.EntityModule;
using Code.InDev.Abilities.Validation.Abstract;
using UnityEngine;

namespace Code.InDev.Abilities.Validation
{
    [CreateAssetMenu(menuName = "Ability/Validators/Caster/Create HealthValidation", fileName = "CasterHealthValidation", order = 0)]
    public class CasterHealthValidation : CasterValidator<Unit, IAbility<Unit>>
    {
        public override bool IsValid(Unit validationObject, IAbility<Unit> ability, ref string errorMessage)
        {
            if (validationObject.IsDead)
            {
                errorMessage = $"Unit is Dead.";

                return false;
            }

            return true;
        }
    }
}