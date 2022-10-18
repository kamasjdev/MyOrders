using MyOrders.Core.Entities;
using Shouldly;

namespace MyOrders.UnitTests.Entities
{
    public class ProductTests
    {
        [Fact]
        public void should_create_product()
        {
            var id = 10;
            var productName = "Product#1";
            var productKind = ProductKind.Create("Kind#1");
            var price = 100M;

            var product = new Product(id, productName, productKind, price);

            product.ShouldNotBeNull();
            product.Id.Value.ShouldBe(id);
            product.ProductName.Value.ShouldBe(productName);
            product.Price.Value.ShouldBe(price);
            product.ProductKind.ShouldNotBeNull();
            product.ProductKind.ProductKindName.ShouldBe(productKind.ProductKindName);
        }
    }
}
