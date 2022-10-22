namespace MyOrders.Application.DTO
{
    public record UpdateOrderDto(int Id, int ProductId, int CustomerId, ISet<int> OrderItemIds);
}
