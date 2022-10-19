using MyOrders.Core.Entities;

namespace MyOrders.Core.Repositories
{
    public interface IContactDataRepository
    {
        Task<ContactData> GetAsync(int id);
        Task<ContactData> AddAsync(ContactData contactData);
        Task<ContactData> UpdateAsync(ContactData contactData);
        Task DeleteAsync(ContactData contactData);
    }
}
