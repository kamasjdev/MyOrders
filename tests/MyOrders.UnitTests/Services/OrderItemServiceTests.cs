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
using Shouldly;
using System.Threading.Tasks;

namespace MyOrders.UnitTests.Services
{
    public class OrderItemServiceTests
    {
        [Fact]
        public async Task should_add_order_item()
        {
            var product = AddDefaultProduct();
            var customer = AddDefaultCustomer();
            var dto = new AddOrderItemDto(product.Id, customer.Id);
            var created = _clock.CurrentDateTime();
            _orderItemRepository.AddAsync(Arg.Any<OrderItem>()).Returns(new OrderItem(1, product, customer, created));

            var orderItem = await _orderItemService.AddAsync(dto);

            orderItem.ShouldNotBeNull();
            orderItem.Product.ShouldNotBeNull();
            orderItem.Product.Id.ShouldBe(product.Id);
            orderItem.Customer.ShouldNotBeNull();
            orderItem.Customer.Id.ShouldBe(customer.Id);
            orderItem.Created.ShouldBe(created);
        }

        [Fact]
        public async Task given_not_existing_product_when_add_should_throw_an_exception()
        {
            var dto = new AddOrderItemDto(1, 1);

            var exception = await Record.ExceptionAsync(() => _orderItemService.AddAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Product");
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task given_not_existing_customer_when_add_should_throw_an_exception()
        {
            var product = AddDefaultProduct();
            var dto = new AddOrderItemDto(product.Id, 1);

            var exception = await Record.ExceptionAsync(() => _orderItemService.AddAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Customer");
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task should_delete_order_item()
        {
            var orderItem = AddDefaultOrderItem();

            await _orderItemService.DeleteAsync(orderItem.Id);

            await _orderItemRepository.Received(1).DeleteAsync(orderItem);
        }

        [Fact]
        public async Task given_not_existing_order_item_when_delete_should_throw_an_exception()
        {
            var id = 1;

            var exception = await Record.ExceptionAsync(() => _orderItemService.DeleteAsync(id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("OrderItem");
            exception.Message.ShouldContain("was not found");
        }

        private Customer AddDefaultCustomer(int? id = null)
        {
            var customer = EntitiesFixtures.CreateDefaultCustomer(id);
            _customerRepository.GetAsync(customer.Id).Returns(customer);
            return customer;
        }

        private Product AddDefaultProduct(int? id = null)
        {
            var product = EntitiesFixtures.CreateDefaultProduct(id);
            _productRepository.GetAsync(product.Id).Returns(product);
            return product;
        }

        private OrderItem AddDefaultOrderItem(int? id = null)
        {
            var orderItem = EntitiesFixtures.CreateDefaultOrderItem();
            _orderItemRepository.GetAsync(orderItem.Id).Returns(orderItem);
            return orderItem;
        }

        private readonly IOrderItemService _orderItemService;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IClock _clock;

        public OrderItemServiceTests()
        {
            _orderItemRepository = Substitute.For<IOrderItemRepository>();
            _productRepository = Substitute.For<IProductRepository>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            _clock = new ClockStub();
            _orderItemService = new OrderItemService(_orderItemRepository, _productRepository, _customerRepository, _clock);
        }
    }
}
