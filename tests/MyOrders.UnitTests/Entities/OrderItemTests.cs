using MyOrders.Core.Entities;
using MyOrders.Core.ValueObjects;
using Shouldly;
using System;

namespace MyOrders.UnitTests.Entities
{
    public class OrderItemTests
    {
        [Fact]
        public void should_create_order_item()
        {
            var product = Product.Create("Product#1", ProductKind.Create("PK#1"), 100M);
            var customer = Customer.Create(Person.From("Test", "Test"), 
                Address.Create(AddressLocation.From("Poland", "City", "Street", 10), "52-123"), 
                ContactData.Create(Email.From("email@email.com"), PhoneNumber.From("+48", "123123123")));
            var created = DateTime.UtcNow;

            var orderItem = OrderItem.Create(product, customer, created);

            orderItem.ShouldNotBeNull();
            orderItem.Product.ShouldNotBeNull();
            orderItem.Customer.ShouldNotBeNull();
            orderItem.Created.ShouldBe(created);
            orderItem.Order.ShouldBeNull();
        }

        [Fact]
        public void should_create_order_item_with_order()
        {
            var product = Product.Create("Product#1", ProductKind.Create("PK#1"), 100M);
            var customer = Customer.Create(Person.From("Test", "Test"),
                Address.Create(AddressLocation.From("Poland", "City", "Street", 10), "25-124"),
                ContactData.Create(Email.From("email@email.com"), PhoneNumber.From("+48", "123123123")));
            var created = DateTime.UtcNow;
            var order = Order.Create("Number", 0M, customer, created);

            var orderItem = OrderItem.Create(product, customer, created, order);

            orderItem.ShouldNotBeNull();
            orderItem.Product.ShouldNotBeNull();
            orderItem.Customer.ShouldNotBeNull();
            orderItem.Created.ShouldBe(created);
            orderItem.Order.ShouldNotBeNull();
        }
    }
}
