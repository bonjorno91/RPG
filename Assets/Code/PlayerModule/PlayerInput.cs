using System.Collections.Generic;
using Code.EntityModule;

namespace Code.PlayerModule
{
    public class PlayerInput : IPlayerInput
    {
        private IPlayerInputStrategy _strategy;
        private readonly HashSet<IEntity> _selectedGroup = new();

        public PlayerInput(IPlayerInputStrategy strategy) => _strategy = strategy;

        public void Handle(IPlayer player)
        {
            _strategy?.Handle(player);
        }

        public void SetStrategy(IPlayerInputStrategy strategy)
        {
            _strategy = strategy;
        }
    }
}