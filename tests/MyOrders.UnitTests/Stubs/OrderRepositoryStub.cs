using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;
using MyOrders.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyOrders.UnitTests.Stubs
{
    internal sealed class OrderRepositoryStub : IOrderRepository
    {
        private readonly RepositoryStub<Order> _repository = new();

        public Task<Order> AddAsync(Order order)
        {
            _repository.Add(order);
            return Task.FromResult(order);
        }

        public Task DeleteAsync(Order order)
        {
            _repository.Delete(order);
            return Task.CompletedTask;
        }

        public Task<Order> GetAsync(int id)
        {
            return Task.FromResult(_repository.Get(id));
        }

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(int customerId)
        {
            await Task.CompletedTask;
            return _repository.GetAll().Where(o => o.Customer.Id == customerId);
        }

        public Task<OrderNumber> GetLastOrderNumberOnDateAsync(DateTime date)
        {
            return Task.FromResult(_repository.GetAll()
                .Where(o => o.Created.Date == date.Date)
                .OrderByDescending(o => o.Created)
                .Select(o => o.OrderNumber)
                .FirstOrDefault());
        }

        public Task<Order> UpdateAsync(Order order)
        {
            return Task.FromResult(order);
        }
    }
}
