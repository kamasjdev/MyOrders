using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;
using MyOrders.Core.ValueObjects;

namespace MyOrders.Infrastructure.DAL.Repositories.InMemory
{
    internal sealed class InMemoryProductKindRepository : IProductKindRepository
    {
        private readonly IInMemoryRepository<ProductKind> _repository;

        public InMemoryProductKindRepository(IInMemoryRepository<ProductKind> repository)
        {
            _repository = repository;
        }

        public Task<ProductKind> AddAsync(ProductKind productKind)
        {
            return Task.FromResult(_repository.Add(productKind));
        }

        public Task DeleteAsync(ProductKind productKind)
        {
            _repository.Delete(productKind);
            return Task.CompletedTask;
        }

        public Task<ProductKind> GetAsync(int id)
        {
            return Task.FromResult(_repository.Get(id));
        }

        public async Task<IEnumerable<ProductKind>> GetAllAsync()
        {
            await Task.CompletedTask;
            return _repository.GetAll();
        }

        public Task<ProductKind> UpdateAsync(ProductKind productKind)
        {
            return Task.FromResult(_repository.Update(productKind));
        }

        public Task<bool> ExistsByProductKindNameAsync(ProductKindName productKindName)
        {
            return Task.FromResult(_repository.GetAll().Any(pk => pk.ProductKindName == productKindName));
        }
    }
}
