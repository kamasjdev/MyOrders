using MyOrders.Application.DTO;

namespace MyOrders.Application.Abstractions
{
    public interface IAddressService
    {
        Task<AddressDto> GetAsync(int id);
        Task<AddressDto> GetByCustomerIdAsync(int customerId);
        Task<AddressDto> AddAsync(AddressDto addressDto);
        Task<AddressDto> UpdateAsync(AddressDto addressDto);
        Task DeleteAsync(int id);
    }
}
