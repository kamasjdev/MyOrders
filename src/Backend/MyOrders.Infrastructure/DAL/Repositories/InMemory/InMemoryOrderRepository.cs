using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;

namespace MyOrders.Infrastructure.DAL.Repositories.InMemory
{
    public sealed class InMemoryOrderRepository : IOrderRepository
    {
        private readonly IInMemoryRepository<Order> _repository;

        public InMemoryOrderRepository(IInMemoryRepository<Order> repository)
        {
            _repository = repository;
        }

        public Task<Order> AddAsync(Order order)
        {
            return Task.FromResult(_repository.Add(order));
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

        public Task<Order> UpdateAsync(Order order)
        {
            return Task.FromResult(_repository.Update(order));
        }
    }
}
