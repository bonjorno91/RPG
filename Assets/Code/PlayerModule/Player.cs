namespace Code.PlayerModule
{
    public class Player : IPlayer
    {
        public string Name { get; }
        public PlayerID ID { get; }
        public bool IsLocal { get; }
        public bool IsAI { get; }
        
        public Relations GetRelationsFor(PlayerID playerID)
        {
            if (playerID == ID)
            {
                return Relations.Ally;
            }

            if (playerID == PlayerID.Aggressive)
            {
                return Relations.Enemy;
            }

            if (playerID == PlayerID.Neutral)
            {
                return Relations.Neutral;
            }

            return Relations.Enemy;
        }

        public Player(string name, PlayerID id,bool isAI, bool isLocal = false)
        {
            ID = id;
            IsAI = isAI;
            Name = name;
            IsLocal = isLocal;
        }
    }
}