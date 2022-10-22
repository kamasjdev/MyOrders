using Microsoft.EntityFrameworkCore;
using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;

namespace MyOrders.Infrastructure.DAL.Repositories
{
    internal sealed class ProductRepository : IProductRepository
    {
        private readonly MyOrdersDbContext _dbContext;

        public ProductRepository(MyOrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product> AddAsync(Product product)
        {
            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task DeleteAsync(Product product)
        {
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public Task<Product> GetAsync(int id)
        {
            return _dbContext.Products.Include(pk => pk.ProductKind).SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }
    }
}
