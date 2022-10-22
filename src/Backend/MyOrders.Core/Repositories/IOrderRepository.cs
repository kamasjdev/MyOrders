using MyOrders.Core.Entities;
using MyOrders.Core.ValueObjects;

namespace MyOrders.Core.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> AddAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task DeleteAsync(Order order);
        Task<Order> GetAsync(int id);
        Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId);
        Task<OrderNumber> GetLastOrderNumberOnDateAsync(DateTime date);
    }
}
