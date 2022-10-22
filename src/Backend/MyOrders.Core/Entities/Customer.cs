using MyOrders.Core.ValueObjects;

namespace MyOrders.Core.Entities
{
    public class Customer : IBaseEntity
    {
        public int Id { get; }
        public Person Person { get; private set; }
        public Address Address { get; private set; }
        public ContactData ContactData { get; private set; }
        public int AddressId { get; private set; }
        public int ContactDataId { get; private set; }

        public IEnumerable<Order> Orders { get; }
        public IEnumerable<OrderItem> OrderItems { get; }

        private Customer()
        { }

        public Customer(int id, Person person, Address address, ContactData contactData)
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
