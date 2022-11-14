namespace Code.PlayerModule
{
    public interface IPlayerController
    {
        IPlayer Player { get; }
        void Update();
    }
}