using Code.EntityModule;
using Code.InDev.Abilities.Validation.Abstract;
using UnityEngine;

namespace Code.InDev.Abilities.Validation
{
    [CreateAssetMenu(menuName = "Ability/Validators/Caster/Create CasterManaValidation", fileName = "CasterManaValidation", order = 0)]
    public class CasterManaValidation : CasterValidator<Unit,IAbility<Unit>>
    {
        public override bool IsValid(Unit validationObject, IAbility<Unit> ability, ref string errorMessage)
        {
            if (validationObject.GetStat(null) > 50)
            {
                errorMessage = "Unit is dead.";
                return false;
            }

            return true;
        }
    }
}