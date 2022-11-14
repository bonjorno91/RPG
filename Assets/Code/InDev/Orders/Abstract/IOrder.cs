using Code.EntityModule;

namespace Code.InDev.Orders.Abstract
{
    /// <summary>
    ///     Marker interface for orders.
    /// </summary>
    public interface IOrder
    {
    }

    public interface IOrder<out TCaster> : IOrder
        where TCaster : IEntity
    {
        public TCaster Caster { get; }
    }

    public interface IOrder<out TCaster, out TTarget> : IOrder<TCaster>
        where TCaster : IEntity
    {
        public TTarget Target { get; }
    }
}