using MyOrders.Core.Exceptions;
using MyOrders.Core.ValueObjects;
using Shouldly;

namespace MyOrders.UnitTests.Entities.ValueObjects
{
    public class ProductKindNameTests
    {
        [Fact]
        public void should_create_product_kind_name()
        {
            var kindName = "ProductKindName";

            var orderNumber = new ProductKindName(kindName);

            orderNumber.ShouldNotBeNull();
            orderNumber.Value.ShouldBe(kindName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void given_invalid_product_kind_name_should_throw_an_exception(string kindName)
        {
            var exception = Record.Exception(() => new ProductKindName(kindName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("Invalid");
        }

        [Theory]
        [InlineData("ab")]
        [InlineData("a")]
        public void given_too_short_product_kind_name_should_throw_an_exception(string kindName)
        {
            var exception = Record.Exception(() => new ProductKindName(kindName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("too short");
        }
    }
}
