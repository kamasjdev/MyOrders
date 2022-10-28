using Microsoft.EntityFrameworkCore;
using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;

namespace MyOrders.Infrastructure.DAL.Repositories
{
    internal sealed class CustomerRepository : ICustomerRepository
    {
        private readonly MyOrdersDbContext _dbContext;

        public CustomerRepository(MyOrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
            await _dbContext.Customers.AddAsync(customer);
            await _dbContext.SaveChangesAsync();
            return customer;
        }

        public async Task DeleteAsync(Customer customer)
        {
            _dbContext.Customers.Remove(customer);
            _dbContext.Addresses.Remove(customer.Address);
            _dbContext.ContactDatas.Remove(customer.ContactData);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _dbContext.Customers.ToListAsync();
        }

        public Task<Customer> GetAsync(int id)
        {
            return _dbContext.Customers
                .Include(a => a.Address)
                .Include(cd => cd.ContactData)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            _dbContext.Customers.Update(customer);
            await _dbContext.SaveChangesAsync();
            return customer;
        }
    }
}
