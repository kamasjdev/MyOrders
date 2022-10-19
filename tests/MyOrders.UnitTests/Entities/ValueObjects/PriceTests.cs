using MyOrders.Core.Exceptions;
using MyOrders.Core.ValueObjects;
using Shouldly;

namespace MyOrders.UnitTests.Entities.ValueObjects
{
    public class PriceTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        public void should_create_price(decimal value)
        {
            var price = new Price(value);

            price.ShouldNotBeNull();
            price.Value.ShouldBe(value);
        }

        [Fact]
        public void given_negative_value_should_throw_an_exception()
        {
            var value = -1;

            var exception = Record.Exception(() => new Price(value));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("is invalid");
        }
    }
}
