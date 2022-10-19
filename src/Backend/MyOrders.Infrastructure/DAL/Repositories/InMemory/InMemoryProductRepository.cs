using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;

namespace MyOrders.Infrastructure.DAL.Repositories.InMemory
{
    public sealed class InMemoryProductRepository : IProductRepository
    {
        private readonly IInMemoryRepository<Product> _repository;

        public InMemoryProductRepository(IInMemoryRepository<Product> repository)
        {
            _repository = repository;
        }

        public Task<Product> AddAsync(Product product)
        {
            return Task.FromResult(_repository.Add(product));
        }

        public Task DeleteAsync(Product product)
        {
            _repository.Delete(product);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            await Task.CompletedTask;
            return _repository.GetAll();
        }

        public Task<Product> GetAsync(int id)
        {
            return Task.FromResult(_repository.Get(id));
        }

        public Task<Product> UpdateAsync(Product product)
        {
            return Task.FromResult(_repository.Update(product));
        }
    }
}
