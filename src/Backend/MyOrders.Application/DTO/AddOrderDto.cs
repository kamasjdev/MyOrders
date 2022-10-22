namespace MyOrders.Application.DTO
{
    public record AddOrderDto(int CustomerId, ISet<int> OrderItemIds);
}
