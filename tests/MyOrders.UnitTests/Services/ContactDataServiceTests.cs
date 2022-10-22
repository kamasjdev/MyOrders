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
    public class ContactDataServiceTests
    {
        [Fact]
        public async Task should_add_contact_data()
        {
            var dto = new ContactDataDto(0, "email@email.com", "+48", "123456789");
            _contactDataRepository.AddAsync(Arg.Any<ContactData>()).Returns(new ContactData(1, dto.Email, PhoneNumber.From(dto.CountryCode, dto.PhoneNumber)));

            var contactData = await _contactDataService.AddAsync(dto);

            contactData.ShouldNotBeNull();
            contactData.Email.ShouldBe(dto.Email);
            contactData.CountryCode.ShouldBe(dto.CountryCode);
            contactData.PhoneNumber.ShouldBe(dto.PhoneNumber);
        }

        [Fact]
        public async Task should_update_contact_data()
        {
            var contactData = AddDefaultContactData();
            var dto = new ContactDataDto(contactData.Id, "email@email.com", "+48", "123456789");
            _contactDataRepository.UpdateAsync(Arg.Any<ContactData>()).Returns(new ContactData(contactData.Id, dto.Email, PhoneNumber.From(dto.CountryCode, dto.PhoneNumber)));

            var contactDataUpdated = await _contactDataService.UpdateAsync(dto);

            contactDataUpdated.ShouldNotBeNull();
            contactDataUpdated.Email.ShouldBe(dto.Email);
            contactDataUpdated.CountryCode.ShouldBe(dto.CountryCode);
            contactDataUpdated.PhoneNumber.ShouldBe(dto.PhoneNumber);
        }

        [Fact]
        public async Task given_not_existing_contact_data_should_throw_an_exception()
        {
            var dto = new ContactDataDto(1, "email@email.com", "+48", "123456789");

            var exception = await Record.ExceptionAsync(() => _contactDataService.UpdateAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task should_delete_contact_data()
        {
            var contactData = AddDefaultContactData();

            await _contactDataService.DeleteAsync(contactData.Id);

            await _contactDataRepository.Received(1).DeleteAsync(contactData);
        }

        [Fact]
        public async Task given_not_existing_contact_data_when_delete_shold_throw_an_exception()
        {
            var id = 1;

            var exception = await Record.ExceptionAsync(() => _contactDataService.DeleteAsync(id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task given_contact_data_with_customer_when_delete_should_throw_an_exception()
        {
            var contactData = AddDefaultContactDataWithCustomer();

            var exception = await Record.ExceptionAsync(() => _contactDataService.DeleteAsync(contactData.Id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Cannot delete ContactData with id");
        }

        private ContactData AddDefaultContactData(int? id = null)
        {
            var contact = EntitiesFixtures.CreateDefaultContactData(id);
            _contactDataRepository.GetAsync(contact.Id).Returns(contact);
            return contact;
        }

        private ContactData AddDefaultContactDataWithCustomer(int? id = null)
        {
            var contact = EntitiesFixtures.CreateDefaultContactDataWithCustomer(id);
            _contactDataRepository.GetAsync(contact.Id).Returns(contact);
            return contact;
        }

        private readonly IContactDataService _contactDataService;
        private readonly IContactDataRepository _contactDataRepository;

        public ContactDataServiceTests()
        {
            _contactDataRepository = Substitute.For<IContactDataRepository>();
            _contactDataService = new ContactDataService(_contactDataRepository);
        }
    }
}
