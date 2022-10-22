using Microsoft.EntityFrameworkCore;
using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;

namespace MyOrders.Infrastructure.DAL.Repositories
{
    internal sealed class AddressRepository : IAddressRepository
    {
        private readonly MyOrdersDbContext _dbContext;

        public AddressRepository(MyOrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Address> AddAsync(Address address)
        {
            await _dbContext.Addresses.AddAsync(address);
            await _dbContext.SaveChangesAsync();
            return address;
        }

        public async Task DeleteAsync(Address address)
        {
            _dbContext.Addresses.Remove(address);
            await _dbContext.SaveChangesAsync();
        }

        public Task<Address> GetAsync(int id)
        {
            return _dbContext.Addresses
                .Include(c => c.Customer)
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Address> UpdateAsync(Address address)
        {
            _dbContext.Addresses.Update(address);
            await _dbContext.SaveChangesAsync();
            return address;
        }
    }
}
