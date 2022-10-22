using MyOrders.Core.ValueObjects;

namespace MyOrders.Core.Entities
{
    public class Address : IBaseEntity
    {
        public int Id { get; }
        public AddressLocation AddressLocation { get; private set; }
        public ZipCode ZipCode { get; private set; }
        public Customer Customer { get; }

        private Address()
        { }

        public Address(int id, AddressLocation addressLocation, ZipCode zipCode)
        {
            Id = id;
            AddressLocation = addressLocation;
            ZipCode = zipCode;
        }

        public static Address Create(AddressLocation addressLocation, ZipCode zipCode)
        {
            return new Address(0, addressLocation, zipCode);
        }

        public void ChangeAddressLocation(AddressLocation addressLocation)
        {
            AddressLocation = addressLocation;
        }

        public void ChangeZipCode(ZipCode zipCode)
        {
            ZipCode = zipCode;
        }
    }
}
