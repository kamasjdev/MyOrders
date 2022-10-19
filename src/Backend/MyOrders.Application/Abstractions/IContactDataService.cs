using MyOrders.Application.DTO;

namespace MyOrders.Application.Abstractions
{
    public interface IContactDataService
    {
        Task<ContactDataDto> GetAsync(int id);
        Task<ContactDataDto> GetByCustomerIdAsync(int customerId);
        Task<ContactDataDto> AddAsync(ContactDataDto contactDataDto);
        Task<ContactDataDto> UpdateAsync(ContactDataDto contactDataDto);
        Task DeleteAsync(int id);
    }
}
