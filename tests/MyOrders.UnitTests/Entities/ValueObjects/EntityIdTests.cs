using MyOrders.Core.Exceptions;
using MyOrders.Core.ValueObjects;
using Shouldly;

namespace MyOrders.UnitTests.Entities.ValueObjects
{
    public class EntityIdTests
    {
        [Fact]
        public void should_create_entity_id()
        {
            var entityId = new EntityId(1);

            entityId.ShouldNotBeNull();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public void given_invalid_entity_id_should_throw_an_exception(int id)
        {
            var exception = Record.Exception(() => new EntityId(id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Invalid");
        }
    }
}
