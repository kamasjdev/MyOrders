namespace MyOrders.Application.DTO
{
    public record UpdateOrderDto(int Id, int CustomerId, ISet<int> OrderItemIds);
}
