namespace Code.InDev.Abilities.Validation.Abstract
{
    public interface ITargetValidator<in TTarget, in TAbility> where TAbility : ITargetAbility<TTarget>
    {
        bool IsValid(TTarget validationObject, TAbility ability,ref string errorMessage);
    }
}