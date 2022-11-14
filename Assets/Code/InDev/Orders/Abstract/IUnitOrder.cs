using Code.EntityModule;

namespace Code.InDev.Orders.Abstract
{
    public interface IUnitOrder<out TTarget> : IOrder<Unit, TTarget>
    {
        
    }
}