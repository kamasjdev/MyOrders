using MyOrders.Core.Exceptions;
using MyOrders.Core.ValueObjects;

namespace MyOrders.Core.Entities
{
    public class Order
    {
        public EntityId Id { get; }
        public OrderNumber OrderNumber { get; private set; }
        public Price Price { get; private set; }
        public Customer Customer { get; private set; }

        public IEnumerable<OrderItem> OrderItems => _orderItems;
        private IList<OrderItem> _orderItems = new List<OrderItem>();

        public Order(EntityId id, OrderNumber orderNumber, Price price, Customer customer, IEnumerable<OrderItem> orderItems = null)
        {
            Id = id;
            OrderNumber = orderNumber;
            Price = price;
            Customer = customer;
            _orderItems = orderItems?.ToList();
        }

        public static Order Create(OrderNumber orderNumber, Price price, Customer customer, IEnumerable<OrderItem> orderItems = null)
        {
            return new Order(0, orderNumber, price, customer, orderItems);
        }

        public void ChangeOrderNumber(OrderNumber orderNumber)
        {
            OrderNumber = orderNumber;
        }

        public void ChangePrice(Price price)
        {
            Price = price;
        }

        public void ChangeCustomer(Customer customer)
        {
            Customer = customer;
        }

        public void AddOrderItem(OrderItem orderItem)
        {
            if (orderItem is null)
            {
                throw new DomainException("OrderItem cannot be null");
            }

            var orderItemToAdd = _orderItems.Where(p => p.Id == orderItem.Id).SingleOrDefault();

            if (orderItemToAdd != null)
            {
                throw new DomainException($"OrderItem with id: '{orderItem.Id}' already exists in order with id: '{Id}'");
            }

            _orderItems.Add(orderItem);
        }

        public void RemoveOrderItem(OrderItem orderItem)
        {
            if (orderItem is null)
            {
                throw new DomainException("OrderItem cannot be null");
            }

            var orderItemToDelete = _orderItems.Where(p => p.Id == orderItem.Id).SingleOrDefault();

            if (orderItemToDelete is null)
            {
                throw new DomainException($"OrderItem with id: '{orderItem.Id}' not found in order with id: '{Id}'");
            }

            _orderItems.Remove(orderItem);
        }
    }
}
