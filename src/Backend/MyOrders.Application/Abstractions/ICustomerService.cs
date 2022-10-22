using MyOrders.Application.DTO;

namespace MyOrders.Application.Abstractions
{
    public interface ICustomerService
    {
        Task<CustomerDetailsDto> GetAsync(int id);
        Task<IEnumerable<CustomerDto>> GetAllAsync();
        Task<CustomerDetailsDto> AddAsync(AddCustomerDto addCustomerDto);
        Task<CustomerDetailsDto> UpdateAsync(UpdateCustomerDto updateCustomerDto);
        Task DeleteAsync(int id);
    }
}
