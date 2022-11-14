using Code.PlayerModule;
using Code.UI;
using UnityEngine;

namespace Code.EntityModule
{
    [SelectionBase]
    public class Item : Entity
    {
        public override EntityTags Tags { get; set; } = EntityTags.Item;

        public override void DrawPanelFor(IEntityPanelProvider entityPanel, IPlayer selectedPlayer)
        {
            entityPanel.InfoPanel.EntityName = gameObject.name;
        }
    }
}