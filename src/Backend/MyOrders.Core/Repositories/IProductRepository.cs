using MyOrders.Core.Entities;

namespace MyOrders.Core.Repositories
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task<Product> GetAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task DeleteAsync(Product product);
    }
}
