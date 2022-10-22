using MyOrders.Core.ValueObjects;

namespace MyOrders.Core.Services
{
    public interface IOrderNumberGenerator
    {
        OrderNumber Generate(OrderNumber lastOrderNumber);
    }
}
