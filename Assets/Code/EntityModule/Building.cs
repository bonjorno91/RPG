using Code.PlayerModule;
using Code.UI;
using UnityEngine;

namespace Code.EntityModule
{
    [SelectionBase]
    public class Building : Unit
    {
        public override void DrawPanelFor(IEntityPanelProvider entityPanel, IPlayer selectedPlayer)
        {
            entityPanel.InfoPanel.EntityName = gameObject.name;
        }
    }
}