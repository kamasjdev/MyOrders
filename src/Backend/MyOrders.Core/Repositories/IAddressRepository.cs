using MyOrders.Core.Entities;

namespace MyOrders.Core.Repositories
{
    public interface IAddressRepository
    {
        Task<Address> AddAsync(Address address);
        Task<Address> UpdateAsync(Address address);
        Task DeleteAsync(Address address);
        Task<Address> GetAsync(int id);
    }
}
