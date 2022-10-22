using MyOrders.Application.Abstractions;
using MyOrders.Application.DTO;
using MyOrders.Application.Exceptions;
using MyOrders.Application.Mappings;
using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;
using MyOrders.Core.Services;

namespace MyOrders.Application.Services
{
    internal sealed class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IClock _clock;

        public OrderItemService(IOrderItemRepository orderItemRepository, IProductRepository productRepository, ICustomerRepository customerRepository, IClock clock)
        {
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _clock = clock;
        }

        public async Task<OrderItemDto> AddAsync(AddOrderItemDto addOrderItemDto)
        {
            var product = await GetProductAsync(addOrderItemDto.ProductId);
            var customer = await GetCustomerAsync(addOrderItemDto.ProductId);
            var orderItem = OrderItem.Create(product, customer, _clock.CurrentDateTime());
            return (await _orderItemRepository.AddAsync(orderItem)).AsDto();
        }

        public async Task DeleteAsync(int id)
        {
            var orderItem = await GetOrderItemAsync(id);
            await _orderItemRepository.DeleteAsync(orderItem);
        }

        public async Task<IEnumerable<OrderItemDto>> GetAllNotOrderedByCustomerIdAsync(int customerId)
        {
            return (await _orderItemRepository.GetAllNotOrderedByCustomerId(customerId)).Select(oi => oi.AsDto());
        }

        private async Task<Product> GetProductAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);

            if (product is null)
            {
                throw new BusinessException($"Product with id: '{id}' was not found");
            }

            return product;
        }

        private async Task<Customer> GetCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetAsync(id);

            if (customer is null)
            {
                throw new BusinessException($"Customer with id: '{id}' was not found");
            }
            
            return customer;
        }

        private async Task<OrderItem> GetOrderItemAsync(int id)
        {
            var orderItem = await _orderItemRepository.GetAsync(id);

            if (orderItem is null)
            {
                throw new BusinessException($"OrderItem with id: '{id}' was not found");
            }

            return orderItem;
        }
    }
}
