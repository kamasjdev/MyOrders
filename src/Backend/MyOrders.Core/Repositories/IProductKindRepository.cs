using MyOrders.Core.Entities;
using MyOrders.Core.ValueObjects;

namespace MyOrders.Core.Repositories
{
    public interface IProductKindRepository
    {
        Task<ProductKind> AddAsync(ProductKind productKind);
        Task<ProductKind> UpdateAsync(ProductKind productKind);
        Task<ProductKind> GetAsync(int id);
        Task DeleteAsync(ProductKind productKind);
        Task<IEnumerable<ProductKind>> GetAllAsync();
        Task<bool> ExistsByProductKindNameAsync(ProductKindName productKindName);
    }
}
