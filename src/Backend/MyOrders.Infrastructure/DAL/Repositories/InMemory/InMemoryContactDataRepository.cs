using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;

namespace MyOrders.Infrastructure.DAL.Repositories.InMemory
{
    internal sealed class InMemoryContactDataRepository : IContactDataRepository
    {
        private readonly IInMemoryRepository<ContactData> _repository;

        public InMemoryContactDataRepository(IInMemoryRepository<ContactData> repository)
        {
            _repository = repository;
        }

        public Task<ContactData> AddAsync(ContactData contactData)
        {
            return Task.FromResult(_repository.Add(contactData));
        }

        public Task DeleteAsync(ContactData contactData)
        {
            _repository.Delete(contactData);
            return Task.CompletedTask;
        }

        public Task<ContactData> GetAsync(int id)
        {
            return Task.FromResult(_repository.Get(id));
        }

        public Task<ContactData> UpdateAsync(ContactData contactData)
        {
            return Task.FromResult(_repository.Update(contactData));
        }
    }
}
