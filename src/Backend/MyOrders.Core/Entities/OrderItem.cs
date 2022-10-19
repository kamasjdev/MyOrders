using MyOrders.Core.ValueObjects;

namespace MyOrders.Core.Entities
{
    public class OrderItem : IBaseEntity
    {
        public EntityId Id { get; }
        public Product Product { get; }
        public Customer Customer { get; }
        public Order Order { get; private set; } = null;
        public DateTime Created { get; }

        public OrderItem(EntityId id, Product product, Customer customer, DateTime created, Order order = null)
        {
            Id = id;
            Product = product;
            Customer = customer;
            Created = created;
            Order = order;
        }

        public static OrderItem Create(Product product, Customer customer, DateTime created, Order order = null)
        {
            return new OrderItem(0, product, customer, created, order: order);
        }
    }
}
