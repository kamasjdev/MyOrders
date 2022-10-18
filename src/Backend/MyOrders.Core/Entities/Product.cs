using MyOrders.Core.ValueObjects;

namespace MyOrders.Core.Entities
{
    public class Product
    {
        public EntityId Id { get; }
        public ProductName ProductName { get; private set; }
        public ProductKind ProductKind { get; private set; }
        public Price Price { get; private set; }

        public Product(EntityId id, ProductName productName, ProductKind productKind, Price price)
        {
            Id = id;
            ProductName = productName;
            ProductKind = productKind;
            Price = price;
        }

        public static Product Create(ProductName productName, ProductKind productKind, Price price)
        {
            return new Product(0, productName, productKind, price);
        }

        public void ChangeProductName(ProductName productName)
        {
            ProductName = productName;
        }

        public void ChangeProductKind(ProductKind productKind)
        {
            ProductKind = productKind;
        }

        public void ChangePrice(Price price)
        {
            Price = price;
        }
    }
}
