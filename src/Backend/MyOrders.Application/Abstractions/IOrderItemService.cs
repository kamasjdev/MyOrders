using MyOrders.Application.DTO;

namespace MyOrders.Application.Abstractions
{
    public interface IOrderItemService
    {
        Task<OrderItemDto> AddAsync(AddOrderItemDto addOrderItemDto);
        Task<IEnumerable<OrderItemDto>> GetAllNotOrderedByCustomerIdAsync(int customerId);
        Task DeleteAsync(int id);
    }
}
