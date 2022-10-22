using MyOrders.Core.Entities;
using MyOrders.Core.ValueObjects;
using System;

namespace MyOrders.UnitTests.Fixtures
{
    internal static class EntitiesFixtures
    {
        public static Address CreateDefaultAddress(int? id = null)
        {
            return new Address(id ?? 1,
                AddressLocation.From("CountryName", "CityName", "StreetName", 1, 1), "ZipCode");
        }

        public static Address CreateDefaultAddressWithCustomer(int? id = null)
        {
            return new Address(id ?? 1,
                AddressLocation.From("CountryName", "CityName", "StreetName", 1, 1), "ZipCode",
                new Customer(1, Person.From("Janusz", "Nosacz"), null, null));
        }

        public static ContactData CreateDefaultContactData(int? id = null)
        {
            return new ContactData(id ?? 1, "email@email.com", PhoneNumber.From("+48", "123456789"));
        }

        public static ContactData CreateDefaultContactDataWithCustomer(int? id = null)
        {
            return new ContactData(id ?? 1, "email@email.com", PhoneNumber.From("+48", "123456789"),
                new Customer(1, Person.From("Janusz", "Nosacz"), null, null));
        }

        public static Customer CreateDefaultCustomer(int? id = null)
        {
            return new Customer(id ?? 1, Person.From("Janusz", "Nosacz"), CreateDefaultAddress(), CreateDefaultContactData());
        }

        public static Product CreateDefaultProduct(int? id = null)
        {
            return new Product(id ?? 1, "Product #1", CreateDefaultProductKind(), 100M);
        }

        public static ProductKind CreateDefaultProductKind(int? id = null)
        {
            return new ProductKind(id ?? 1, "ProductKind #1");
        }

        public static OrderItem CreateDefaultOrderItem(int? id = null)
        {
            return new OrderItem(id ?? 1, CreateDefaultProduct(), CreateDefaultCustomer(), DateTime.UtcNow);
        }
    }
}
