using MyOrders.Core.ValueObjects;

namespace MyOrders.Core.Entities
{
    public class Customer : IBaseEntity
    {
        public EntityId Id { get; }
        public Person Person { get; private set; }
        public Address Address { get; private set; }
        public ContactData ContactData { get; private set; }

        public Customer(EntityId id, Person person, Address address, ContactData contactData)
        {
            Id = id;
            Person = person;
            Address = address;
            ContactData = contactData;
        }

        public static Customer Create(Person person, Address address, ContactData contactData)
        {
            return new Customer(0, person, address, contactData);
        }

        public void ChangePerson(Person person)
        {
            Person = person;
        }

        public void ChangeAddress(Address address)
        {
            Address = address;
        }

        public void ChangeContactData(ContactData contactData)
        {
            ContactData = contactData;
        }
    }
}
