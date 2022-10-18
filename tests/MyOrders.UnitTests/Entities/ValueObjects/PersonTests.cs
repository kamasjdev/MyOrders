using MyOrders.Core.Exceptions;
using MyOrders.Core.ValueObjects;
using Shouldly;

namespace MyOrders.UnitTests.Entities.ValueObjects
{
    public class PersonTests
    {
        [Fact]
        public void should_create_person()
        {
            var firstName = "Fist";
            var lastName = "Last";

            var person = Person.From(firstName, lastName);

            person.ShouldNotBeNull();
            person.FirstName.ShouldBe(firstName);
            person.LastName.ShouldBe(lastName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void given_invalid_first_name_should_throw_an_exception(string firstName)
        {
            var lastName = "Last";

            var exception = Record.Exception(() => Person.From(firstName, lastName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.Contains("Invalid");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void given_invalid_last_name_should_throw_an_exception(string lastName)
        {
            var firstName = "Last";

            var exception = Record.Exception(() => Person.From(firstName, lastName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.Contains("Invalid");
        }

        [Theory]
        [InlineData("a")]
        [InlineData("ab")]
        public void given_too_short_first_name_should_throw_an_exception(string firstName)
        {
            var lastName = "Las";

            var exception = Record.Exception(() => Person.From(firstName, lastName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.Contains("too short");
        }

        [Theory]
        [InlineData("a")]
        [InlineData("ab")]
        public void given_too_short_last_name_should_throw_an_exception(string lastName)
        {
            var firstName = "Fir";

            var exception = Record.Exception(() => Person.From(firstName, lastName));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.Contains("too short");
        }
    }
}
