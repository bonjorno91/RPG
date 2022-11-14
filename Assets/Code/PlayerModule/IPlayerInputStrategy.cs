namespace Code.PlayerModule
{
    public interface IPlayerInputStrategy
    {
        void Handle(IPlayer player);
        void Cancel();
    }
}