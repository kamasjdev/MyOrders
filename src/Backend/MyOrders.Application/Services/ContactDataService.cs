using MyOrders.Application.Abstractions;
using MyOrders.Application.DTO;
using MyOrders.Application.Exceptions;
using MyOrders.Application.Mappings;
using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;
using MyOrders.Core.ValueObjects;

namespace MyOrders.Application.Services
{
    internal sealed class ContactDataService : IContactDataService
    {
        private readonly IContactDataRepository _contactDataRepository;

        public ContactDataService(IContactDataRepository contactDataRepository)
        {
            _contactDataRepository = contactDataRepository;
        }

        public async Task<ContactDataDto> AddAsync(ContactDataDto contactDataDto)
        {
            var contact = ContactData.Create(contactDataDto.Email, PhoneNumber.From(contactDataDto.CountryCode, contactDataDto.PhoneNumber));
            return (await _contactDataRepository.AddAsync(contact)).AsDto();
        }

        public async Task DeleteAsync(int id)
        {
            var contactData = await GetContactDataAsync(id);

            if (contactData.Customer is not null)
            {
                throw new BusinessException($"Cannot delete ContactData with id: '{contactData.Id}', because is used by Customer with id: '{contactData.Customer.Id}'");
            }

            await _contactDataRepository.DeleteAsync(contactData);
        }

        public async Task<ContactDataDto> GetAsync(int id)
        {
            return (await _contactDataRepository.GetAsync(id))?.AsDto();
        }

        public async Task<ContactDataDto> UpdateAsync(ContactDataDto contactDataDto)
        {
            var contactData = await GetContactDataAsync(contactDataDto.Id);
            contactData.ChangeEmail(contactDataDto.Email);
            contactData.ChangePhoneNumber(PhoneNumber.From(contactDataDto.CountryCode, contactDataDto.PhoneNumber));
            return (await _contactDataRepository.UpdateAsync(contactData)).AsDto();
        }

        private async Task<ContactData> GetContactDataAsync(int id)
        {
            var contactData = await _contactDataRepository.GetAsync(id);

            if (contactData is null)
            {
                throw new BusinessException($"ContactData with id: '{id}' was not found");
            }

            return contactData;
        }
    }
}
