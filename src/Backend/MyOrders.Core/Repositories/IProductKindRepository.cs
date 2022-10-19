using MyOrders.Core.Entities;

namespace MyOrders.Core.Repositories
{
    public interface IProductKindRepository
    {
        Task<ProductKind> AddAsync(ProductKind productKind);
        Task<ProductKind> UpdateAsync(ProductKind productKind);
        Task<ProductKind> GetAsync(int id);
        Task DeleteAsync(ProductKind productKind);
    }
}
