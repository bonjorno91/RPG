using Code.EntityModule;

namespace Code.InDev.Abilities.Validation.Abstract
{
    public interface ICasterValidator<in TObject, in TAbility> where TObject : IEntity where TAbility : IAbility<TObject>
    {
        bool IsValid(TObject validationObject, TAbility ability, ref string errorMessage);
    }
}