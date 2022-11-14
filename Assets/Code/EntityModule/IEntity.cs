using Code.InDev.Orders.Abstract;
using Code.PlayerModule;
using Code.UI;
using UnityEngine;

namespace Code.EntityModule
{
    public interface IEntity
    {
        PlayerID OwnerPlayerID { get; }
        EntityTags Tags { get; }
        Transform Origin { get; }
        void IssueOrder<TOrder>(TOrder order) where TOrder : IOrder;
        void DrawPanelFor(IEntityPanelProvider entityPanel, IPlayer selectedPlayer);
    }
}