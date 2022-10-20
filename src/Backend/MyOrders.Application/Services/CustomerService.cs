using MyOrders.Application.Abstractions;
using MyOrders.Application.DTO;
using MyOrders.Application.Exceptions;
using MyOrders.Application.Mappings;
using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;
using MyOrders.Core.ValueObjects;

namespace MyOrders.Application.Services
{
    internal class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IContactDataRepository _contactDataRepository;

        public CustomerService(ICustomerRepository customerRepository, IAddressRepository addressRepository, IContactDataRepository contactDataRepository)
        {
            _customerRepository = customerRepository;
            _addressRepository = addressRepository;
            _contactDataRepository = contactDataRepository;
        }

        public async Task<CustomerDto> AddAsync(AddCustomerDto addCustomerDto)
        {
            var address = await GetAddressAsync(addCustomerDto.AddressId);
            var contactData = await GetContactDataAsync(addCustomerDto.ContactDataId);
            var customer = Customer.Create(Person.From(addCustomerDto.FirstName, addCustomerDto.LastName), address, contactData);
            return (await _customerRepository.AddAsync(customer)).AsDetailsDto();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await GetCustomerAsync(id);
            await _customerRepository.DeleteAsync(customer);
        }

        public async Task<IEnumerable<CustomerDto>> GetAll()
        {
            return (await _customerRepository.GetAllAsync()).Select(c => c.AsDto());
        }

        public async Task<CustomerDetailsDto> GetAsync(int id)
        {
            return (await _customerRepository.GetAsync(id))?.AsDetailsDto();
        }

        public async Task<CustomerDto> UpdateAsync(UpdateCustomerDto updateCustomerDto)
        {
            var customer = await GetCustomerAsync(updateCustomerDto.Id);
            var address = await GetAddressAsync(updateCustomerDto.AddressId);
            var contactData = await GetContactDataAsync(updateCustomerDto.ContactDataId);
            customer.ChangePerson(Person.From(updateCustomerDto.FirstName, updateCustomerDto.LastName));
            customer.ChangeAddress(address);
            customer.ChangeContactData(contactData);
            return (await _customerRepository.UpdateAsync(customer))?.AsDto();
        }

        private async Task<Address> GetAddressAsync(int id)
        {
            var address = await _addressRepository.GetAsync(id);

            if (address is null)
            {
                throw new BusinessException($"Address with id: '{id}' was not found");
            }
            
            return address;
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

        private async Task<Customer> GetCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetAsync(id);

            if (customer is null)
            {
                throw new BusinessException($"Customer with id: '{id}' was not found");
            }

            return customer;
        }
    }
}
