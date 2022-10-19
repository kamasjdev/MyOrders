using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;

namespace MyOrders.Infrastructure.DAL.Repositories.InMemory
{
    public sealed class InMemoryProductKindRepository : IProductKindRepository
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

        public Task<ProductKind> UpdateAsync(ProductKind productKind)
        {
            return Task.FromResult(_repository.Update(productKind));
        }
    }
}
