using Microsoft.EntityFrameworkCore;
using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;

namespace MyOrders.Infrastructure.DAL.Repositories
{
    internal sealed class ContactDataRepository : IContactDataRepository
    {
        private readonly MyOrdersDbContext _dbContext;

        public ContactDataRepository(MyOrdersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ContactData> AddAsync(ContactData contactData)
        {
            await _dbContext.ContactDatas.AddAsync(contactData);
            await _dbContext.SaveChangesAsync();
            return contactData;
        }

        public async Task DeleteAsync(ContactData contactData)
        {
            _dbContext.ContactDatas.Remove(contactData);
            await _dbContext.SaveChangesAsync();
        }

        public Task<ContactData> GetAsync(int id)
        {
            return _dbContext.ContactDatas
                .Include(c => c.Customer)
                .SingleOrDefaultAsync(cd => cd.Id == id);
        }

        public async Task<ContactData> UpdateAsync(ContactData contactData)
        {
            _dbContext.ContactDatas.Update(contactData);
            await _dbContext.SaveChangesAsync();
            return contactData;
        }
    }
}
