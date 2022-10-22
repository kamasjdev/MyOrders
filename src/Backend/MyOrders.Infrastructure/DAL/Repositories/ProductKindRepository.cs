using Microsoft.EntityFrameworkCore;
using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;
using MyOrders.Core.ValueObjects;

namespace MyOrders.Infrastructure.DAL.Repositories
{
    internal sealed class ProductKindRepository : IProductKindRepository
    {
        private readonly MyOrdersDbContext _dbContext;

        public ProductKindRepository(MyOrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductKind> AddAsync(ProductKind productKind)
        {
            await _dbContext.ProductKinds.AddAsync(productKind);
            await _dbContext.SaveChangesAsync();
            return productKind;
        }

        public async Task DeleteAsync(ProductKind productKind)
        {
            _dbContext.ProductKinds.Remove(productKind);
            await _dbContext.SaveChangesAsync();
        }

        public Task<bool> ExistsByProductKindNameAsync(ProductKindName productKindName)
        {
            return _dbContext.ProductKinds.AnyAsync(pk => pk.ProductKindName == productKindName);
        }

        public async Task<IEnumerable<ProductKind>> GetAllAsync()
        {
            return await _dbContext.ProductKinds.ToListAsync();
        }

        public Task<ProductKind> GetAsync(int id)
        {
            return _dbContext.ProductKinds.SingleOrDefaultAsync(pk => pk.Id == id);
        }

        public async Task<ProductKind> UpdateAsync(ProductKind productKind)
        {
            _dbContext.ProductKinds.Update(productKind);
            await _dbContext.SaveChangesAsync();
            return productKind;
        }
    }
}
