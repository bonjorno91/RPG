namespace Code.InDev.Orders.Abstract
{
    public interface IOrderHandler<in TOrder> where TOrder : IOrder
    {
        /// <summary>
        /// Executor for specific order.
        /// </summary>
        /// <param name="order"></param>
        /// <returns>Execution success result.</returns>
        bool ExecuteOrder(TOrder order);
    }
}