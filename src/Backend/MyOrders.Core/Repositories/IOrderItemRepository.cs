using MyOrders.Core.Entities;

namespace MyOrders.Core.Repositories
{
    public interface IOrderItemRepository
    {
        Task<OrderItem> AddAsync(OrderItem orderItem);
        Task<IEnumerable<OrderItem>> GetAllNotOrderedByCustomerId(int customerId);
        Task DeleteAsync(OrderItem orderItem);
        Task<OrderItem> GetAsync(int id);
    }
}
