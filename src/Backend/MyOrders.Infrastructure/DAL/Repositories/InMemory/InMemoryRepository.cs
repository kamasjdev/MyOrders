using MyOrders.Core.Entities;
using MyOrders.Core.ValueObjects;
using System.Reflection;

namespace MyOrders.Infrastructure.DAL.Repositories.InMemory
{
    internal sealed class InMemoryRepository<T> : IInMemoryRepository<T>
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
            return _entities.Max(e => e.Id);
        }

        private static void SetId(T entity, int id)
        {
            var field = typeof(T).GetField($"<{nameof(IBaseEntity.Id)}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(entity, new EntityId(id));
        }
    }
}
