using MyOrders.Core.ValueObjects;

namespace MyOrders.Core.Entities
{
    public class OrderItem : IBaseEntity
    {
        public int Id { get; }
        public Product Product { get; private set; }
        public Customer Customer { get; private set; }
        public Order Order { get; private set; } = null;
        public DateTime Created { get; }

        private OrderItem()
        { }

        public OrderItem(int id, Product product, Customer customer, DateTime created, Order order = null)
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
