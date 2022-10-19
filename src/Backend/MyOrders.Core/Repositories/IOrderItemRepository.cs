using MyOrders.Core.Entities;

namespace MyOrders.Core.Repositories
{
    public interface IOrderItemRepository
    {
        Task<OrderItem> AddAsync(OrderItem orderItem);
        Task<IEnumerable<OrderItem>> GetAllByCustomerId(int customerId);
        Task DeleteAsync(OrderItem orderItem);
    }
}
