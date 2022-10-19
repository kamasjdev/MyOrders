namespace MyOrders.Application.DTO
{
    public record AddOrderDto(string OrderNumber, int ProductId, int CustomerId, ISet<int> OrderItemIds);
}
