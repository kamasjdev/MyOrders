using MyOrders.Core.ValueObjects;

namespace MyOrders.Core.Entities
{
    public class Address
    {
        public EntityId Id { get; }
        public AddressLocation AddressLocation { get; private set; }

        public Address(EntityId id, AddressLocation addressLocation)
        {
            Id = id;
            AddressLocation = addressLocation;
        }

        public static Address Create(AddressLocation addressLocation)
        {
            return new Address(0, addressLocation);
        }

        public void ChangeAddressLocation(AddressLocation addressLocation)
        {
            AddressLocation = addressLocation;
        }
    }
}
