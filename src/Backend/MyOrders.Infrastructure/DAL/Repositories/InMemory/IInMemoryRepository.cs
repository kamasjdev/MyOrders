using MyOrders.Core.Entities;

namespace MyOrders.Infrastructure.DAL.Repositories.InMemory
{
    public interface IInMemoryRepository<T>
        where T : class, IBaseEntity
    {
        T Add(T entity);
        T Update(T entity);
        T Get(int id);
        IEnumerable<T> GetAll();
        void Delete(T entity);
    }
}
