using MyOrders.Application.Abstractions;
using MyOrders.Application.DTO;
using MyOrders.Application.Exceptions;
using MyOrders.Application.Services;
using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;
using MyOrders.Core.ValueObjects;
using MyOrders.UnitTests.Fixtures;
using NSubstitute;
using Shouldly;
using System.Threading.Tasks;

namespace MyOrders.UnitTests.Services
{
    public class CustomerServiceTests
    {
        [Fact]
        public async Task should_add_customer()
        {
            var address = AddDefaultAddress();
            var contact = AddDefaultContactData();
            var dto = new AddCustomerDto("Janusz", "Nosacz", address.Id, contact.Id);
            _customerRepository.AddAsync(Arg.Any<Customer>()).Returns(new Customer(1, Person.From(dto.FirstName, dto.LastName), address, contact));

            var customer = await _customerService.AddAsync(dto);

            customer.ShouldNotBeNull();
            customer.FirstName.ShouldBe(dto.FirstName);
            customer.LastName.ShouldBe(dto.LastName);
            customer.ContactData.ShouldNotBeNull();
            customer.ContactData.Id.ShouldBe(dto.ContactDataId);
            customer.Address.ShouldNotBeNull();
            customer.Address.Id.ShouldBe(dto.AddressId);
        }

        [Fact]
        public async Task given_not_existing_address_when_add_should_throw_an_exception()
        {
            var dto = new AddCustomerDto("Janusz", "Nosacz", 1, 1);

            var exception = await Record.ExceptionAsync(() => _customerService.AddAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Address");
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task given_not_existing_contact_data_when_add_should_throw_an_exception()
        {
            var address = AddDefaultAddress();
            var dto = new AddCustomerDto("Janusz", "Nosacz", address.Id, 1);

            var exception = await Record.ExceptionAsync(() => _customerService.AddAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("ContactData");
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task should_update_contact_data()
        {
            var customer = AddDefaultCustomer();
            var address = AddDefaultAddress();
            var contact = AddDefaultContactData();
            var dto = new UpdateCustomerDto(customer.Id, "Tester", "Abcde", address.Id, contact.Id);
            _customerRepository.UpdateAsync(Arg.Any<Customer>()).Returns(new Customer(1, Person.From(dto.FirstName, dto.LastName), address, contact));

            var customerUpdated = await _customerService.UpdateAsync(dto);

            customerUpdated.ShouldNotBeNull();
            customerUpdated.FirstName.ShouldBe(dto.FirstName);
            customerUpdated.LastName.ShouldBe(dto.LastName);
            customerUpdated.ContactData.ShouldNotBeNull();
            customerUpdated.ContactData.Id.ShouldBe(dto.ContactDataId);
            customerUpdated.Address.ShouldNotBeNull();
            customerUpdated.Address.Id.ShouldBe(dto.AddressId);
        }

        [Fact]
        public async Task given_not_existing_customer_when_update_should_throw_an_exception()
        {
            var dto = new UpdateCustomerDto(1, "Janusz", "Nosacz", 1, 1);

            var exception = await Record.ExceptionAsync(() => _customerService.UpdateAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Customer");
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task given_not_existing_address_when_update_should_throw_an_exception()
        {
            var customer = AddDefaultCustomer();
            var dto = new UpdateCustomerDto(customer.Id, "Janusz", "Nosacz", 1, 1);

            var exception = await Record.ExceptionAsync(() => _customerService.UpdateAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Address");
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task given_not_existing_contact_data_when_update_should_throw_an_exception()
        {
            var customer = AddDefaultCustomer();
            var address = AddDefaultAddress();
            var dto = new UpdateCustomerDto(customer.Id, "Janusz", "Nosacz", address.Id, 1);

            var exception = await Record.ExceptionAsync(() => _customerService.UpdateAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("ContactData");
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task should_delete_customer()
        {
            var customer = AddDefaultCustomer();

            await _customerService.DeleteAsync(customer.Id);

            await _customerRepository.Received(1).DeleteAsync(customer);
        }

        [Fact]
        public async Task given_not_existing_customer_should_throw_an_exception()
        {
            var id = 1;

            var exception = await Record.ExceptionAsync(() => _customerService.DeleteAsync(id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Customer");
            exception.Message.ShouldContain("was not found");
        }

        private Address AddDefaultAddress(int? id = null)
        {
            var address = EntitiesFixtures.CreateDefaultAddress(id);
            _addressRepository.GetAsync(address.Id).Returns(address);
            return address;
        }

        private ContactData AddDefaultContactData(int? id = null)
        {
            var contact = EntitiesFixtures.CreateDefaultContactData(id);
            _contactDataRepository.GetAsync(contact.Id).Returns(contact);
            return contact;
        }

        private Customer AddDefaultCustomer(int? id = null)
        {
            var customer = EntitiesFixtures.CreateDefaultCustomer(id);
            _customerRepository.GetAsync(customer.Id).Returns(customer);
            return customer;
        }

        private readonly ICustomerService _customerService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IContactDataRepository _contactDataRepository;

        public CustomerServiceTests()
        {
            _customerRepository = Substitute.For<ICustomerRepository>();
            _addressRepository = Substitute.For<IAddressRepository>();
            _contactDataRepository = Substitute.For<IContactDataRepository>();
            _customerService = new CustomerService(_customerRepository, _addressRepository, _contactDataRepository);
        }
    }
}
