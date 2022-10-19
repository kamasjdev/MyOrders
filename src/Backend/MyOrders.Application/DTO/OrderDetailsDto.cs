namespace MyOrders.Application.DTO
{
    public record OrderDetailsDto : OrderDto
    {
        public CustomerDto Customer { get; init; }
        public IEnumerable<OrderItemDto> OrderItems { get; init; }

        public OrderDetailsDto(int id, string orderNumber, decimal price, DateTime created, DateTime? modified, CustomerDto customer, IEnumerable<OrderItemDto> orderItems)
            : base(id, orderNumber, price, created, modified)
        {
            Customer = customer;
            OrderItems = orderItems;
        }
    }
}
