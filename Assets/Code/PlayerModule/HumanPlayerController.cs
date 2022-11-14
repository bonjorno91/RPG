namespace Code.PlayerModule
{
    public class HumanPlayerController : IPlayerController
    {
        public IPlayer Player { get; }
        private readonly IPlayerInput _input;
        
        public HumanPlayerController(IPlayer player,IPlayerInput input)
        {
            _input = input;
            Player = player;
        }

        public void Update()
        {
            _input.Handle(Player);
        }
    }
}