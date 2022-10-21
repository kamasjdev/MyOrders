using MyOrders.Core.Entities;
using MyOrders.Core.Exceptions;
using MyOrders.Core.ValueObjects;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyOrders.UnitTests.Entities
{
    public class OrderTests
    {
        [Fact]
        public void should_create_order()
        {
            var orderNumber = "Order/123";
            var price = 100M;
            var customer = Customer.Create(Person.From("Test", "Test"), Address.Create(AddressLocation.From("Poland", "City", "Street", 10), "12-123"),
                ContactData.Create(Email.From("email@email.com"), PhoneNumber.From("+48", "123456789")));
            var created = DateTime.UtcNow;

            var order = Order.Create(orderNumber, price, customer, created);

            order.ShouldNotBeNull();
            order.Customer.ShouldNotBeNull();
            order.Price.Value.ShouldBe(price);
            order.Created.ShouldBe(created);
        }

        [Fact]
        public void should_create_order_with_order_items()
        {
            var orderNumber = "Order/123";
            var customer = Customer.Create(Person.From("Test", "Test"), Address.Create(AddressLocation.From("Poland", "City", "Street", 10), "08-912"),
                ContactData.Create(Email.From("email@email.com"), PhoneNumber.From("+48", "123456789")));
            var created = DateTime.UtcNow;
            var orderItems = new List<OrderItem> { OrderItem.Create(Product.Create("Product#1", ProductKind.Create("PKind"), 100M), customer, created),
                OrderItem.Create(Product.Create("Product#1", ProductKind.Create("PKind"), 100M), customer, created) };
            var price = orderItems.Sum(oi => oi.Product.Price.Value);

            var order = Order.Create(orderNumber, price, customer, created, orderItems);

            order.ShouldNotBeNull();
            order.Customer.ShouldNotBeNull();
            order.Price.Value.ShouldBe(price);
            order.Created.ShouldBe(created);
            order.OrderItems.ShouldNotBeEmpty();
            order.OrderItems.Count().ShouldBe(orderItems.Count);
        }

        [Fact]
        public void given_valid_order_with_new_order_number_should_change_order_number()
        {
            var order = CreateDefaultOrder();
            var newOrderNumber = "Order/123/123";
            var modified = DateTime.UtcNow;

            order.ChangeOrderNumber(newOrderNumber, modified);

            order.OrderNumber.Value.ShouldBe(newOrderNumber);
            order.Modified.ShouldBe(modified);
        }

        [Fact]
        public void given_valid_order_with_new_price_should_change_price()
        {
            var order = CreateDefaultOrder();
            var priceWithTax = order.Price.Value * 1.23M;
            var modified = DateTime.UtcNow;

            order.ChangePrice(priceWithTax, modified);

            order.Price.Value.ShouldBe(priceWithTax);
            order.Modified.ShouldBe(modified);
        }

        [Fact]
        public void given_valid_order_with_new_customer_should_change_customer()
        {
            var order = CreateDefaultOrder();
            var customer = Customer.Create(Person.From("Person", "Person"), Address.Create(AddressLocation.From("USA", "CityA", "StreetB", 101), "12-634"),
                ContactData.Create(Email.From("email2@email2.com"), PhoneNumber.From("+12", "987654321")));
            var modified = DateTime.UtcNow;

            order.ChangeCustomer(customer, modified);

            order.Customer.Person.ShouldBe(customer.Person);
            order.Customer.Address.Id.ShouldBe(customer.Address.Id);
            order.Customer.Address.AddressLocation.ShouldBe(customer.Address.AddressLocation);
            order.Customer.ContactData.Id.ShouldBe(customer.ContactData.Id);
            order.Customer.ContactData.Email.ShouldBe(customer.ContactData.Email);
            order.Customer.ContactData.PhoneNumber.ShouldBe(customer.ContactData.PhoneNumber);
            order.Modified.ShouldBe(modified);
        }

        [Fact]
        public void given_valid_order_item_should_add_to_order()
        {
            var created = DateTime.UtcNow;
            var order = CreateDefaultOrder();
            var modified = created.AddHours(1);
            var orderItem = CreateDefaultOrderItem(0);
            var orderItemsCount = order.OrderItems.Count();
            var expectedPrice = order.Price + orderItem.Product.Price;

            order.AddOrderItem(orderItem, modified);

            order.OrderItems.Count().ShouldBe(orderItemsCount + 1);
            order.Price.Value.ShouldBe(expectedPrice);
        }

        [Fact]
        public void given_existing_order_item_when_add_to_order_should_throw_an_exception()
        {
            var order = CreateDefaultOrder();
            var orderItem = order.OrderItems.First();

            var exception = Record.Exception(() => order.AddOrderItem(orderItem, DateTime.UtcNow));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("already exists in order");
        }

        [Fact]
        public void given_invalid_order_item_when_add_to_order_should_throw_an_exception()
        {
            var order = CreateDefaultOrder();

            var exception = Record.Exception(() => order.AddOrderItem(null, DateTime.UtcNow));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("OrderItem cannot be null");
        }

        [Fact]
        public void given_valid_order_item_should_remove_from_order()
        {
            var order = CreateDefaultOrder();
            var orderItemToDelete = order.OrderItems.First();
            var priceBeforeRemoveOrderItem = order.Price.Value;
            var count = order.OrderItems.Count();
            var modified = DateTime.UtcNow;

            order.RemoveOrderItem(orderItemToDelete, modified);

            order.Price.Value.ShouldBeLessThan(priceBeforeRemoveOrderItem);
            order.OrderItems.Count().ShouldBe(count - 1);
        }

        [Fact]
        public void given_invalid_order_item_should_throw_an_exception_when_remove_from_order()
        {
            var order = CreateDefaultOrder();

            var exception = Record.Exception(() => order.RemoveOrderItem(null, DateTime.UtcNow));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("OrderItem cannot be null");
        }

        [Fact]
        public void given_not_existing_order_item_should_throw_an_exception_when_remove_from_order()
        {
            var order = CreateDefaultOrder();
            var orderItem = new OrderItem(99999, Product.Create("product", ProductKind.Create("PK12"), 100M),
                Customer.Create(Person.From("First", "Last"), Address.Create(AddressLocation.From("ABCD", "EFG", "HIJK", 1), "90-123"),
                        ContactData.Create(Email.From("email@emial.com"), PhoneNumber.From("+48", "123456789"))), DateTime.UtcNow);

            var exception = Record.Exception(() => order.RemoveOrderItem(orderItem, DateTime.UtcNow));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("not found in order");
        }

        public Order CreateDefaultOrder()
        {
            var orderNumber = "Order/123";
            var customer = Customer.Create(Person.From("Test", "Test"), Address.Create(AddressLocation.From("Poland", "City", "Street", 10), "12-123"),
                ContactData.Create(Email.From("email@email.com"), PhoneNumber.From("+48", "123456789")));
            var created = DateTime.UtcNow;
            var orderItems = new List<OrderItem> { CreateDefaultOrderItem(), CreateDefaultOrderItem() };
            var price = orderItems.Sum(oi => oi.Product.Price.Value);
            return new Order(1, orderNumber, price, customer, created, null, orderItems);
        }

        public OrderItem CreateDefaultOrderItem(int? id = null)
        {
            var customer = Customer.Create(Person.From("Test", "Test"), Address.Create(AddressLocation.From("Poland", "City", "Street", 10), "62-124"),
                   ContactData.Create(Email.From("email@email.com"), PhoneNumber.From("+48", "123456789")));
            var created = DateTime.UtcNow;
            return new OrderItem(id ?? new Random().Next(1, 1000) , Product.Create("Product#1", ProductKind.Create("PKind"), 100M), customer, created);
        }
    }
}
