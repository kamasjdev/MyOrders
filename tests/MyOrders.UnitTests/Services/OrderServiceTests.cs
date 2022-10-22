using MyOrders.Application.Abstractions;
using MyOrders.Application.DTO;
using MyOrders.Application.Exceptions;
using MyOrders.Application.Services;
using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;
using MyOrders.Core.Services;
using MyOrders.UnitTests.Fixtures;
using MyOrders.UnitTests.Stubs;
using NSubstitute;
using NSubstitute.Extensions;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyOrders.UnitTests.Services
{
    public class OrderServiceTests
    {
        [Fact]
        public async Task should_add_order()
        {
            var orderNumber = _orderNumberGenerator.Generate(null);
            var customer = AddDefaultCustomer();
            var dto = new AddOrderDto(customer.Id, new HashSet<int>());

            var orderAdded = await _orderService.AddAsync(dto);

            orderAdded.ShouldNotBeNull();
            orderAdded.Customer.ShouldNotBeNull();
            orderAdded.Customer.Id.ShouldBe(dto.CustomerId);
            orderAdded.OrderNumber.ShouldNotBeNullOrWhiteSpace();
            orderAdded.OrderNumber.ShouldBe(orderNumber);
            orderAdded.Price.ShouldBe(decimal.Zero);
            orderAdded.Modified.ShouldBeNull();
        }

        [Fact]
        public async Task should_add_order_with_order_items()
        {
            var orderNumber = _orderNumberGenerator.Generate(null);
            var customer = AddDefaultCustomer();
            var orderItem = AddDefaultOrderItem();
            var orderItem2 = AddDefaultOrderItem(2);
            var expectedPrice = orderItem.Product.Price + orderItem2.Product.Price;
            var dto = new AddOrderDto(customer.Id, new HashSet<int> { orderItem.Id, orderItem2.Id });

            var orderAdded = await _orderService.AddAsync(dto);

            orderAdded.ShouldNotBeNull();
            orderAdded.Customer.ShouldNotBeNull();
            orderAdded.Customer.Id.ShouldBe(dto.CustomerId);
            orderAdded.OrderNumber.ShouldNotBeNullOrWhiteSpace();
            orderAdded.OrderNumber.ShouldBe(orderNumber);
            orderAdded.Price.ShouldBe(expectedPrice);
            orderAdded.Modified.ShouldNotBeNull();
        }

        [Fact]
        public async Task given_not_exisiting_customer_when_add_should_throw_an_exception()
        {
            var dto = new AddOrderDto(1, new HashSet<int>());

            var exception = await Record.ExceptionAsync(() => _orderService.AddAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Customer");
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task given_not_exisiting_order_item_when_add_should_throw_an_exception()
        {
            var customer = AddDefaultCustomer();
            var dto = new AddOrderDto(customer.Id, new HashSet<int> { 1 });

            var exception = await Record.ExceptionAsync(() => _orderService.AddAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("OrderItem");
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task should_update_order()
        {
            var order = await AddDefaultOrder();
            var orderItemAdded = order.OrderItems.First();
            var orderItem = AddDefaultOrderItem(3);
            var customer = AddDefaultCustomer(5);
            var dto = new UpdateOrderDto(order.Id, customer.Id, new HashSet<int> { orderItemAdded.Id, orderItem.Id });

            var orderUpdated = await _orderService.UpdateAsync(dto);

            orderUpdated.ShouldNotBeNull();
            orderUpdated.Customer.ShouldNotBeNull();
            orderUpdated.Customer.Id.ShouldBe(customer.Id);
            orderUpdated.OrderItems.ShouldNotBeEmpty();
            orderUpdated.OrderItems.Count().ShouldBe(dto.OrderItemIds.Count);
            orderUpdated.OrderItems.ShouldContain(oi => oi.Id == orderItemAdded.Id);
            orderUpdated.OrderItems.ShouldContain(oi => oi.Id == orderItem.Id);
        }

        [Fact]
        public async Task given_not_existing_order_when_update_should_throw_an_exception()
        {
            var dto = new UpdateOrderDto(1, 1, new HashSet<int>());

            var exception = await Record.ExceptionAsync(() => _orderService.UpdateAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Order");
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task given_not_existing_customer_when_update_should_throw_an_exception()
        {
            var order = await AddDefaultOrder();
            var dto = new UpdateOrderDto(order.Id, 1, new HashSet<int>());

            var exception = await Record.ExceptionAsync(() => _orderService.UpdateAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Customer");
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task given_not_existing_order_item_when_update_should_throw_an_exception()
        {
            var order = await AddDefaultOrder();
            var customer = AddDefaultCustomer();
            var dto = new UpdateOrderDto(order.Id, customer.Id, new HashSet<int> { 5 });

            var exception = await Record.ExceptionAsync(() => _orderService.UpdateAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("OrderItem");
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task should_update_order_price()
        {
            var order = await AddDefaultOrder();
            var price = 2000M;
            var dto = new UpdateOrderPriceDto(order.Id, price);

            var orderUpdated = await _orderService.UpdatePriceAsync(dto);

            orderUpdated.ShouldNotBeNull();
            orderUpdated.Price.ShouldBe(price);
        }

        [Fact]
        public async Task given_not_existing_order_when_update_price_should_throw_an_exception()
        {
            var price = 2000M;
            var dto = new UpdateOrderPriceDto(1, price);

            var exception = await Record.ExceptionAsync(() => _orderService.UpdatePriceAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Order");
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task should_delete_order()
        {
            var order = AddDefaultOrder();

            await _orderService.DeleteAsync(order.Id);

            var orderDeleted = await _orderRepository.GetAsync(order.Id);
            orderDeleted.ShouldBeNull();
        }

        [Fact]
        public async Task given_not_existing_order_when_delete_should_throw_an_exception()
        {
            var exception = await Record.ExceptionAsync(() => _orderService.DeleteAsync(1));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Order");
            exception.Message.ShouldContain("was not found");
        }

        private Customer AddDefaultCustomer(int? id = null)
        {
            var customer = EntitiesFixtures.CreateDefaultCustomer(id);
            _customerRepository.GetAsync(customer.Id).Returns(customer);
            return customer;
        }

        private OrderItem AddDefaultOrderItem(int? id = null)
        {
            var orderItem = EntitiesFixtures.CreateDefaultOrderItem(id);
            _orderItemRepository.GetAsync(orderItem.Id).Returns(orderItem);
            return orderItem;
        }

        private Task<Order> AddDefaultOrder()
        {
            var orderItem = EntitiesFixtures.CreateDefaultOrderItem();
            var orderItem2 = EntitiesFixtures.CreateDefaultOrderItem(2);
            var customer = EntitiesFixtures.CreateDefaultCustomer();
            var orderNumber = _orderNumberGenerator.Generate(null);
            return _orderRepository.AddAsync(Order.Create(orderNumber, orderItem.Product.Price, customer, _clock.CurrentDateTime(), new List<OrderItem> { orderItem, orderItem2 }));
        }

        private readonly IOrderService _orderService;
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IClock _clock;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderNumberGenerator _orderNumberGenerator;

        public OrderServiceTests()
        {
            _orderRepository = new OrderRepositoryStub();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _clock = new ClockStub();
            _orderItemRepository = Substitute.For<IOrderItemRepository>();
            _orderNumberGenerator = new OrderNumberGenerator(_clock);
            _orderService = new OrderService(_orderRepository, _customerRepository, _clock, _orderItemRepository, _orderNumberGenerator);
        }
    }
}
