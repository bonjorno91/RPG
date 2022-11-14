namespace Code.PlayerModule
{
    public interface IPlayerInput
    {
        void Handle(IPlayer player);
        void SetStrategy(IPlayerInputStrategy strategy);
    }
}