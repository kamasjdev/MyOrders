namespace MyOrders.Application.DTO
{
    public record UpdateOrderDto(int Id, string OrderNumber, int ProductId, int CustomerId, ISet<int> OrderItemIds);
}
