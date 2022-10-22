using MyOrders.Core.Entities;
using MyOrders.Core.ValueObjects;
using Shouldly;

namespace MyOrders.UnitTests.Entities
{
    public class ContactDataTests
    {
        [Fact]
        public void should_create_contact_data()
        {
            var id = 1;
            var email = Email.From("email@email.com");
            var phoneNumber = PhoneNumber.From("+48", "1234567");

            var contactData = new ContactData(id, email, phoneNumber);

            contactData.ShouldNotBeNull();
            contactData.Id.ShouldBe(id);
            contactData.Email.ShouldBe(email);
            contactData.PhoneNumber.ShouldBe(phoneNumber);
        }
    }
}
