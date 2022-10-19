using MyOrders.Core.Exceptions;
using MyOrders.Core.ValueObjects;
using Shouldly;

namespace MyOrders.UnitTests.Entities.ValueObjects
{
    public class PhoneNumberTests
    {
        [Fact]
        public void should_create_phone_number()
        {
            var countryCode = "+48";
            var numbers = "123456789";

            var phoneNumber = PhoneNumber.From(countryCode, numbers);

            phoneNumber.ShouldNotBeNull();
            phoneNumber.CountryCode.ShouldBe(countryCode);
            phoneNumber.Numbers.ShouldBe(numbers);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("+a23")]
        public void given_invalid_country_code_should_throw_an_exception(string countryCode)
        {
            var numbers = "123456789";

            var exception = Record.Exception(() => PhoneNumber.From(countryCode, numbers));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("Invalid CountryCode");
        }

        [Fact]
        public void given_invalid_format_country_code_should_throw_an_exception()
        {
            var countryCode = "48";
            var numbers = "123456789";

            var exception = Record.Exception(() => PhoneNumber.From(countryCode, numbers));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("Invalid format CountryCode");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("abc")]
        public void given_invalid_numbers_should_throw_an_exception(string numbers)
        {
            var countryCode = "+48";

            var exception = Record.Exception(() => PhoneNumber.From(countryCode, numbers));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("Invalid Numbers");
        }
    }
}
