using MyOrders.Core.Entities;

namespace MyOrders.Core.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> AddAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task DeleteAsync(Order order);
        Task<Order> GetAsync(int id);
        Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId);
    }
}
