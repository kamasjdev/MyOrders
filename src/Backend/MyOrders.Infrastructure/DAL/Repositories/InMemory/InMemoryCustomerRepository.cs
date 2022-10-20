using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;

namespace MyOrders.Infrastructure.DAL.Repositories.InMemory
{
    internal sealed class InMemoryCustomerRepository : ICustomerRepository
    {
        private readonly IInMemoryRepository<Customer> _repository;

        public InMemoryCustomerRepository(IInMemoryRepository<Customer> repository)
        {
            _repository = repository;
        }

        public Task<Customer> AddAsync(Customer customer)
        {
            _repository.Add(customer);
            return Task.FromResult(customer);
        }

        public Task DeleteAsync(Customer customer)
        {
            _repository.Delete(customer);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            await Task.CompletedTask;
            return _repository.GetAll();
        }

        public Task<Customer> GetAsync(int id)
        {
            return Task.FromResult(_repository.Get(id));
        }

        public Task<Customer> UpdateAsync(Customer customer)
        {
            return Task.FromResult(_repository.Update(customer));
        }
    }
}
