using MyOrders.Core.Exceptions;
using MyOrders.Core.Services;
using MyOrders.UnitTests.Stubs;
using Shouldly;

namespace MyOrders.UnitTests.Services
{
    public class OrderNumberGeneratorTests
    {
        [Theory]
        [InlineData("ORDER/2022/10/22/10")]
        [InlineData("ORDER/2012/10/22/100")]
        [InlineData("ORDER/2002/10/22/10000")]
        public void should_generate_order_number(string lastOrderNumber)
        {
            var lastNumber = long.Parse(lastOrderNumber[17..]);

            var orderNumber = _orderNumberGenerator.Generate(lastOrderNumber);

            orderNumber.ShouldNotBeNull();
            orderNumber.Value.ShouldNotBeNullOrWhiteSpace();
            orderNumber.Value.ShouldContain("ORDER");
            var currentNumber = long.Parse(orderNumber.Value[17..]);
            currentNumber.ShouldBeGreaterThan(lastNumber);
        }

        [Fact]
        public void should_generate_order_number_when_null_last_order_number_passed()
        {
            var orderNumber = _orderNumberGenerator.Generate(null);

            orderNumber.ShouldNotBeNull();
            orderNumber.Value.ShouldNotBeNullOrWhiteSpace();
            orderNumber.Value.ShouldContain("ORDER");
            var currentNumber = long.Parse(orderNumber.Value[17..]);
            currentNumber.ShouldBe(1);
        }

        [Theory]
        [InlineData("ORDER/2022/10/22/10abcas")]
        [InlineData("OR/2022/10/22/10")]
        [InlineData("ORDER/2012/10/")]
        [InlineData("ORDER/2002/")]
        public void given_invalid_last_order_number_should_throw_an_exception(string lastOrderNumber)
        {
            var exception = Record.Exception(() => _orderNumberGenerator.Generate(lastOrderNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Given invalid OrderNumber");
        }

        private readonly IClock _clock;
        private readonly IOrderNumberGenerator _orderNumberGenerator;

        public OrderNumberGeneratorTests()
        {
            _clock = new ClockStub();
            _orderNumberGenerator = new OrderNumberGenerator(_clock);
        }
    }
}
