using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;

namespace MyOrders.Infrastructure.DAL.Repositories.InMemory
{
    internal sealed class InMemoryOrderItemRepository : IOrderItemRepository
    {
        private readonly IInMemoryRepository<OrderItem> _repository;

        public InMemoryOrderItemRepository(IInMemoryRepository<OrderItem> repository)
        {
            _repository = repository;
        }

        public Task<OrderItem> AddAsync(OrderItem orderItem)
        {
            return Task.FromResult(_repository.Add(orderItem));
        }

        public Task DeleteAsync(OrderItem orderItem)
        {
            _repository.Delete(orderItem);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<OrderItem>> GetAllByCustomerId(int customerId)
        {
            await Task.CompletedTask;
            return _repository.GetAll().Where(oi => oi.Customer.Id == customerId);
        }

        public Task<OrderItem> GetAsync(int id)
        {
            return Task.FromResult(_repository.Get(id));
        }
    }
}
