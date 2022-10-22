using MyOrders.Core.Entities;
using MyOrders.Core.ValueObjects;

namespace MyOrders.Core.Repositories
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task<Product> GetAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task DeleteAsync(Product product);
        Task<bool> ExistsByProductNameAsync(ProductName productName);
    }
}
