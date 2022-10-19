namespace MyOrders.Application.DTO
{
    public record ProductDto(int Id, string ProductName, ProductKindDto ProductKind, decimal Price);
}
