using MyOrders.Application.Abstractions;
using MyOrders.Application.DTO;
using MyOrders.Application.Exceptions;
using MyOrders.Application.Mappings;
using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;
using MyOrders.Core.Services;

namespace MyOrders.Application.Services
{
    internal sealed class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IClock _clock;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderNumberGenerator _orderNumberGenerator;

        public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository, IClock clock,
            IOrderItemRepository orderItemRepository, IOrderNumberGenerator orderNumberGenerator)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _clock = clock;
            _orderItemRepository = orderItemRepository;
            _orderNumberGenerator = orderNumberGenerator;
        }

        public async Task<OrderDetailsDto> AddAsync(AddOrderDto addOrderDto)
        {
            var customer = await GetCustomerAsync(addOrderDto.CustomerId);
            var currentDateTime = _clock.CurrentDateTime();
            var lastOrderNumber = await _orderRepository.GetLastOrderNumberOnDateAsync(currentDateTime);
            var orderNumber = _orderNumberGenerator.Generate(lastOrderNumber);
            var order = Order.Create(orderNumber, decimal.Zero, customer, currentDateTime);

            foreach(var orderItemId in addOrderDto.OrderItemIds)
            {
                var orderItem = await GetOrderItemAsync(orderItemId);
                order.AddOrderItem(orderItem, currentDateTime);
            }

            return (await _orderRepository.AddAsync(order)).AsDetailsDto();
        }

        public async Task DeleteAsync(int id)
        {
            var order = await GetOrderAsync(id);
            await _orderRepository.DeleteAsync(order);
        }

        public async Task<OrderDetailsDto> GetAsync(int id)
        {
            return (await _orderRepository.GetAsync(id))?.AsDetailsDto();
        }

        public async Task<IEnumerable<OrderDto>> GetByCustomerIdAsync(int customerId)
        {
            return (await _orderRepository.GetByCustomerIdAsync(customerId)).Select(o => o.AsDto());
        }

        public async Task<OrderDetailsDto> UpdateAsync(UpdateOrderDto updateOrderDto)
        {
            var order = await GetOrderAsync(updateOrderDto.Id);
            var customer = await GetCustomerAsync(updateOrderDto.CustomerId);
            var currentDateTime = _clock.CurrentDateTime();
            order.ChangeCustomer(customer, currentDateTime);

            foreach (var orderItemId in updateOrderDto.OrderItemIds)
            {
                var orderItemExists = order.OrderItems.Any(oi => oi.Id == orderItemId);

                if (orderItemExists)
                {
                    continue;
                }

                var orderItem = await GetOrderItemAsync(orderItemId);
                order.AddOrderItem(orderItem, currentDateTime);
            }

            var orderItems = new List<OrderItem>(order.OrderItems);
            foreach(var orderItem in orderItems)
            {
                var orderItemExists = updateOrderDto.OrderItemIds.Any(oi => oi == orderItem.Id);

                if (orderItemExists)
                {
                    continue;
                }

                order.RemoveOrderItem(orderItem, currentDateTime);
            }

            return (await _orderRepository.UpdateAsync(order)).AsDetailsDto();
        }

        public async Task<OrderDto> UpdatePriceAsync(UpdateOrderPriceDto updateOrderPriceDto)
        {
            var order = await GetOrderAsync(updateOrderPriceDto.Id);
            order.ChangePrice(updateOrderPriceDto.Price, _clock.CurrentDateTime());
            return (await _orderRepository.UpdateAsync(order)).AsDetailsDto();
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

        private async Task<Order> GetOrderAsync(int id)
        {
            var order = await _orderRepository.GetAsync(id);

            if (order is null)
            {
                throw new BusinessException($"Order with id: '{id}' was not found");
            }

            return order;
        }
    }
}
