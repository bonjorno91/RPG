using Code.EntityModule;
using Code.InDev.Orders.Abstract;

namespace Code.InDev.Orders
{
    public interface ISmartOrder<out TTarget> : IOrder<IEntity, TTarget>
    {
        
    }
}