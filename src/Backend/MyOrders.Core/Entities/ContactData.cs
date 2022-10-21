using MyOrders.Core.ValueObjects;

namespace MyOrders.Core.Entities
{
    public class ContactData : IBaseEntity
    {
        public EntityId Id { get; }
        public Email Email { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; } = null;
        public Customer Customer { get; }

        private ContactData()
        { }

        public ContactData(EntityId id, Email email, PhoneNumber phoneNumber)
        {
            Id = id;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public static ContactData Create(Email email, PhoneNumber phoneNumber)
        {
            return new ContactData(0, email, phoneNumber);
        }

        public void ChangeEmail(Email email)
        {
            Email = email;
        }

        public void ChangePhoneNumber(PhoneNumber phoneNumber)
        {
            PhoneNumber = phoneNumber;
        }
    }
}
