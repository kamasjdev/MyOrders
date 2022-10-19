using MyOrders.Application.DTO;

namespace MyOrders.Application.Abstractions
{
    public interface IOrderService
    {
        Task<OrderDetailsDto> AddAsync(AddOrderDto addOrderDto);
        Task<OrderDetailsDto> UpdateAsync(UpdateOrderDto updateOrderDto);
        Task<OrderDto> UpdatePriceAsync(UpdateOrderPriceDto updateOrderPriceDto);
        Task DeleteAsync(int id);
        Task<OrderDetailsDto> GetAsync(int id);
        Task<IEnumerable<OrderDto>> GetByCustomerIdAsync(int customerId);
    }
}
