using MyOrders.Core.Exceptions;
using MyOrders.Core.ValueObjects;
using Shouldly;

namespace MyOrders.UnitTests.Entities.ValueObjects
{
    public class OrderNumberTests
    {
        [Fact]
        public void should_create_order_number()
        {
            var number = "Order";

            var orderNumber = new OrderNumber(number);

            orderNumber.ShouldNotBeNull();
            orderNumber.Value.ShouldBe(number);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void given_invalid_order_number_should_throw_an_exception(string number)
        {
            var exception = Record.Exception(() => new OrderNumber(number));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("Invalid");
        }

        [Theory]
        [InlineData("ab")]
        [InlineData("a")]
        public void given_too_short_order_number_should_throw_an_exception(string number)
        {
            var exception = Record.Exception(() => new OrderNumber(number));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("too short");
        }
    }
}
