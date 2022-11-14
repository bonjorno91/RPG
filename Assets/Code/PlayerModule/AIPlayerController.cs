namespace Code.PlayerModule
{
    public class AIPlayerController : IPlayerController
    {
        public IPlayer Player { get; }

        public AIPlayerController(IPlayer player)
        {
            Player = player;
        }

        public void Update()
        {
            
        }
    }
}