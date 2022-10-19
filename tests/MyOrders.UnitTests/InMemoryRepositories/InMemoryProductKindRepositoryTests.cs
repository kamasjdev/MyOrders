using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;
using MyOrders.Core.ValueObjects;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyOrders.UnitTests.InMemoryRepositories
{
    public class InMemoryProductKindRepositoryTests
    {
        private readonly IProductKindRepository _productKindRepository;

        [Fact]
        public async Task should_create_product_kind()
        {
            var productKind = ProductKind.Create("Prod");

            var productKindAdded = await _productKindRepository.AddAsync(productKind);

            productKindAdded.ShouldNotBeNull();
            productKindAdded.Id.Value.ShouldBeGreaterThan(0);
        } 

        [Fact]
        public async Task should_create_product_kinds()
        {
            var productKind1 = ProductKind.Create("Prod");
            var productKind2 = ProductKind.Create("Prod");

            var productKindAdded1 = await _productKindRepository.AddAsync(productKind1);
            var productKindAdded2 = await _productKindRepository.AddAsync(productKind2);

            productKindAdded1.ShouldNotBeNull();
            productKindAdded1.Id.Value.ShouldBeGreaterThan(0);
            productKindAdded2.ShouldNotBeNull();
            productKindAdded2.Id.Value.ShouldBeGreaterThan(0);
        } 

        public InMemoryProductKindRepositoryTests()
        {
            _productKindRepository = new InMemoryProductKindRepository();
        }
    }

    public sealed class InMemoryProductKindRepository : IProductKindRepository
    {
        private readonly InMemoryRepository<ProductKind> _repository;

        public InMemoryProductKindRepository()
        {
            _repository = new InMemoryRepository<ProductKind>();
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

    internal sealed class InMemoryRepository<T>
        where T : class, IBaseEntity
    {
        private readonly IList<T> _entities = new List<T>();

        public T Add(T entity)
        {
            var lastId = GetLastId();
            SetId(entity, lastId + 1);
            _entities.Add(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            _entities.Remove(entity);
        }

        public IEnumerable<T> GetAll()
            => _entities;

        public T Get(int id)
            => _entities.SingleOrDefault(e => e.Id == id);

        public T Update(T entity)
        {
            return entity;
        }

        private int GetLastId()
        {
            if (!_entities.Any())
            {
                return 0;
            }

            return _entities.Max(e => e.Id);
        }

        private static void SetId(T entity, int id)
        {
            var field = typeof(T).GetField($"<{nameof(IBaseEntity.Id)}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(entity, new EntityId(id));
        }
    }
}
