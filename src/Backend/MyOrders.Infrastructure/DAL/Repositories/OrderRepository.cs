using Microsoft.EntityFrameworkCore;
using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;
using MyOrders.Core.ValueObjects;

namespace MyOrders.Infrastructure.DAL.Repositories
{
    internal sealed class OrderRepository : IOrderRepository
    {
        private readonly MyOrdersDbContext _dbContext;

        public OrderRepository(MyOrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Order> AddAsync(Order order)
        {
            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task DeleteAsync(Order order)
        {
            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
        }

        public Task<OrderNumber> GetLastOrderNumberOnDateAsync(DateTime date)
        {
            return _dbContext.Orders
                .Where(o => o.Created.Date == date.Date)
                .OrderByDescending(o => o.Created)
                .Select(o => o.OrderNumber)
                .FirstOrDefaultAsync();
        }

        public Task<Order> GetAsync(int id)
        {
            return _dbContext.Orders
                .Include(c => c.Customer)
                .Include(oi => oi.OrderItems).ThenInclude(p => p.Product)
                .SingleOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId)
        {
            return await _dbContext.Orders.Where(o => o.Customer.Id == customerId).ToListAsync();
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }
    }
}
