namespace Code.PlayerModule
{
    public interface IPlayer
    {
        string Name { get; }
        PlayerID ID { get; }
        bool IsLocal { get; }
        bool IsAI { get; }
        Relations GetRelationsFor(PlayerID playerID);
    }

    public enum Relations
    {
        Ally,Enemy,Neutral,Unknown
    }
    
}