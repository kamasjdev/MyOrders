using MyOrders.Application.DTO;

namespace MyOrders.Application.Abstractions
{
    public interface ICustomerService
    {
        Task<CustomerDetailsDto> GetAsync(int id);
        Task<CustomerDto> GetAll();
        Task<CustomerDto> AddAsync(AddCustomerDto addCustomerDto);
        Task<CustomerDto> UpdateAsync(UpdateCustomerDto updateCustomerDto);
        Task DeleteAsync(int id);
    }
}
