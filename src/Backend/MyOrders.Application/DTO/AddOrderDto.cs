namespace MyOrders.Application.DTO
{
    public record AddOrderDto(int ProductId, int CustomerId, ISet<int> OrderItemIds);
}
