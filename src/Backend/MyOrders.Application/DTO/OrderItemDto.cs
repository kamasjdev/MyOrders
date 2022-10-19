namespace MyOrders.Application.DTO
{
    public record OrderItemDto(int Id, ProductDto Product, CustomerDto Customer, DateTime Created);
}
