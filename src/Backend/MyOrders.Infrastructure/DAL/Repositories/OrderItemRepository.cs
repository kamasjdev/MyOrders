using Microsoft.EntityFrameworkCore;
using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;

namespace MyOrders.Infrastructure.DAL.Repositories
{
    internal sealed class OrderItemRepository : IOrderItemRepository
    {
        private readonly MyOrdersDbContext _dbContext;

        public OrderItemRepository(MyOrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderItem> AddAsync(OrderItem orderItem)
        {
            await _dbContext.OrderItems.AddAsync(orderItem);
            await _dbContext.SaveChangesAsync();
            return orderItem;
        }

        public async Task DeleteAsync(OrderItem orderItem)
        {
            _dbContext.OrderItems.Remove(orderItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetAllNotOrderedByCustomerId(int customerId)
        {
            return await _dbContext.OrderItems
                                .Include(c => c.Customer)
                                .Include(p => p.Product)
                                .Include(o => o.Order)
                                .Where(oi => oi.Order == null && oi.Customer.Id == customerId)
                                .ToListAsync();
        }

        public Task<OrderItem> GetAsync(int id)
        {
            return _dbContext.OrderItems
                .Include(p => p.Product)
                .Include(c => c.Customer)
                .SingleOrDefaultAsync(oi => oi.Id == id);
        }
    }
}
