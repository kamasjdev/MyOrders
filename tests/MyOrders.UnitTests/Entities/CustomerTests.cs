using MyOrders.Core.Entities;
using MyOrders.Core.ValueObjects;
using Shouldly;

namespace MyOrders.UnitTests.Entities
{
    public class CustomerTests
    {
        [Fact]
        public void should_create_customer()
        {
            var id = 20;
            var person = Person.From("Test", "Test2");
            var address = Address.Create(AddressLocation.From("Poland", "Warszawa", "Dluga", 2), "01-023");
            var contactData = ContactData.Create("email@email.com", PhoneNumber.From("+48", "1234567"));

            var customer = new Customer(id, person, address, contactData);

            customer.ShouldNotBeNull();
            customer.Id.Value.ShouldBe(id);
            customer.Person.ShouldBe(person);
            customer.Address.AddressLocation.ShouldBe(address.AddressLocation);
            customer.ContactData.Email.ShouldBe(contactData.Email);
            customer.ContactData.PhoneNumber.ShouldBe(contactData.PhoneNumber);
        }
    }
}
