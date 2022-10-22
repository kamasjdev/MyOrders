namespace MyOrders.Application.DTO
{
    public record ProductDetailsDto : ProductDto
    {
        public ProductKindDto ProductKind { get; init; }

        public ProductDetailsDto(int id, string productName, ProductKindDto productKind, decimal price) : base(id, productName, price)
        {
            ProductKind = productKind;
        }
    }
}
