namespace MyOrders.Application.DTO
{
    public record OrderDto(int Id, string OrderNumber, decimal Price, DateTime Created, DateTime? Modified);
}
