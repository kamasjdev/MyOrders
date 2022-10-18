using MyOrders.Core.Entities;
using MyOrders.Core.ValueObjects;
using Shouldly;

namespace MyOrders.UnitTests.Entities
{
    public class AddressTests
    {
        [Fact]
        public void should_create_address()
        {
            var id = 5;
            var addressLocation = AddressLocation.From("Poland", "Zielona Gora", "Szafrana", 50);

            var address = new Address(id, addressLocation);

            address.ShouldNotBeNull();
            address.Id.Value.ShouldBe(id);
            address.AddressLocation.ShouldBe(addressLocation);
        }
    }
}
