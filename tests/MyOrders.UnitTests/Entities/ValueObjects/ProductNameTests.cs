using MyOrders.Core.Exceptions;
using MyOrders.Core.ValueObjects;
using Shouldly;

namespace MyOrders.UnitTests.Entities.ValueObjects
{
    public class ProductNameTests
    {
        [Fact]
        public void should_create_product_name()
        {
            var name = "ProductName";

            var orderNumber = new ProductName(name);

            orderNumber.ShouldNotBeNull();
            orderNumber.Value.ShouldBe(name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void given_invalid_product_name_should_throw_an_exception(string name)
        {
            var exception = Record.Exception(() => new ProductName(name));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("Invalid");
        }

        [Theory]
        [InlineData("ab")]
        [InlineData("a")]
        public void given_too_short_product_kind_name_should_throw_an_exception(string name)
        {
            var exception = Record.Exception(() => new ProductName(name));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("too short");
        }
    }
}
