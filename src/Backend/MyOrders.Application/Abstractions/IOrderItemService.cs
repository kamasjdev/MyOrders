using MyOrders.Application.DTO;

namespace MyOrders.Application.Abstractions
{
    public interface IOrderItemService
    {
        Task<OrderItemDto> AddAsync(AddOrderItemDto addOrderItemDto);
        Task<IEnumerable<OrderItemDto>> GetAllByCustomerId(int customerId);
        Task DeleteAsync(int id);
    }
}
