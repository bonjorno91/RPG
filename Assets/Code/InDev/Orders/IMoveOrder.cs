using Code.InDev.Orders.Abstract;

namespace Code.InDev.Orders
{
    public interface IMoveOrder<out TTarget> : IUnitOrder<TTarget>
    {
        
    }
}