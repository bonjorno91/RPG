using Code.PlayerModule;
using Code.UI;
using UnityEngine;

namespace Code.EntityModule
{
    [SelectionBase]
    public class Destructible : Entity
    {
        public override EntityTags Tags { get; set; } = EntityTags.Destructible;

        public override void DrawPanelFor(IEntityPanelProvider entityPanel, IPlayer selectedPlayer)
        {
            throw new System.NotImplementedException();
        }
    }
}