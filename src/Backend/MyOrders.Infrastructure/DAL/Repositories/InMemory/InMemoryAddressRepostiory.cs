using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;

namespace MyOrders.Infrastructure.DAL.Repositories.InMemory
{
    public sealed class InMemoryAddressRepostiory : IAddressRepository
    {
        private readonly IInMemoryRepository<Address> _repository;

        public InMemoryAddressRepostiory(IInMemoryRepository<Address> repository)
        {
            _repository = repository;
        }

        public async Task<Address> AddAsync(Address address)
        {
            await Task.CompletedTask;
            return _repository.Add(address);
        }

        public Task DeleteAsync(Address address)
        {
            _repository.Delete(address);
            return Task.CompletedTask;
        }

        public Task<Address> GetAsync(int id)
        {
            return Task.FromResult(_repository.Get(id));
        }

        public Task<Address> UpdateAsync(Address address)
        {
            _repository.Update(address);
            return Task.FromResult(address);
        }
    }
}
