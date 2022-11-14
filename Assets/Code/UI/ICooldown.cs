namespace Code.ActionsModule.Abilities
{
    public interface ICooldown
    {
        void SetProgress(float progressClamped);
    }
}