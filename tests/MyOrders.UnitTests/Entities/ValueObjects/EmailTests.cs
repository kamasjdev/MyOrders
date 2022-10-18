using MyOrders.Core.Exceptions;
using MyOrders.Core.ValueObjects;
using Shouldly;

namespace MyOrders.UnitTests.Entities.ValueObjects
{
    public class EmailTests
    {
        [Fact]
        public void should_create_email()
        {
            var emailName = "test@test.com";

            var email = Email.From(emailName);

            email.Value.ShouldBe(emailName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void given_null_empty_or_with_white_space_email_should_throw_an_exception(string emailName)
        {
            var exception = Record.Exception(() => Email.From(emailName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("Invalid");
        }

        [Theory]
        [InlineData("email")]
        [InlineData("email@")]
        public void given_invalid_email_should_throw_an_exception(string emailName)
        {
            var exception = Record.Exception(() => Email.From(emailName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("is invalid");
            exception.Message.ShouldContain(emailName);
        }
    }
}
