using MyOrders.Core.Entities;
using Shouldly;

namespace MyOrders.UnitTests.Entities
{
    public class ProductKindTests
    {
        public void should_create_product_kind()
        {
            var id = 1;
            var name = "kind";

            var productKind = new ProductKind(id, name);

            productKind.ShouldNotBeNull();
            productKind.Id.ShouldBe(id);
            productKind.ProductKindName.Value.ShouldBe(name);
        }
    }
}
