using MyOrders.Core.Entities;

namespace MyOrders.Core.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> GetAsync(int id);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> AddAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task DeleteAsync(Customer customer);
    }
}
