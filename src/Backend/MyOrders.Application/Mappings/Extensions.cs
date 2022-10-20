using MyOrders.Application.DTO;
using MyOrders.Core.Entities;

namespace MyOrders.Application.Mappings
{
    internal static class Extensions
    {
        public static AddressDto AsDto(this Address address)
        {
            return new AddressDto(address.Id, address.AddressLocation.StreetName, address.AddressLocation.CityName, address.AddressLocation.CountryName,
                address.AddressLocation.BuildingNumber, address.AddressLocation.FlatNumber);
        }

        public static ContactDataDto AsDto(this ContactData contactData)
        {
            return new ContactDataDto(contactData.Id, contactData.Email, contactData.PhoneNumber.CountryCode, contactData.PhoneNumber.Numbers);
        }

        public static CustomerDto AsDto(this Customer customer)
        {
            return new CustomerDto(customer.Id, customer.Person.FirstName, customer.Person.LastName);
        }

        public static CustomerDetailsDto AsDetailsDto(this Customer customer)
        {
            return new CustomerDetailsDto(customer.Id, customer.Person.FirstName, customer.Person.LastName, customer.Address.AsDto(), customer.ContactData.AsDto());
        }

        public static OrderItemDto AsDto(this OrderItem orderItem)
        {
            return new OrderItemDto(orderItem.Id, orderItem.Product.AsDto(), orderItem.Customer.AsDto(), orderItem.Created);
        }

        public static OrderDto AsDto(this Order order)
        {
            return new OrderDto(order.Id, order.OrderNumber.Value, order.Price, order.Created, order.Modified);
        }

        public static OrderDetailsDto AsDetailsDto(this Order order)
        {
            return new OrderDetailsDto(order.Id, order.OrderNumber.Value, order.Price, order.Created, order.Modified, order.Customer.AsDto(), order.OrderItems.Select(oi => oi.AsDto()));
        }

        public static ProductDto AsDto(this Product product)
        {
            return new ProductDto(product.Id, product.ProductName.Value, product.ProductKind.AsDto(), product.Price);
        }

        public static ProductKindDto AsDto(this ProductKind productKind)
        {
            return new ProductKindDto(productKind.Id, productKind.ProductKindName.Value);
        }
    }
}
