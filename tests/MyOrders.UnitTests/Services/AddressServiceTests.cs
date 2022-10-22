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
    public class AddressServiceTests
    {
        [Theory]
        [InlineData("Poland", "Zielona Góra", "Wrocławska", 10, 1, "65-510")]
        [InlineData("Poland", "Zielona Góra", "Szafrana", 20, null, "65-520")]
        public async Task should_add_address(string countryName, string cityName, string streetName, int buildingNumber, int? flatNumber, string zipCode)
        {
            var dto = new AddressDto(0, streetName, cityName, countryName, buildingNumber, zipCode, flatNumber);
            _addressRepository.AddAsync(Arg.Any<Address>()).Returns(new Address(1, AddressLocation.From(dto.CountryName, dto.CityName, dto.StreetName, dto.BuildingNumber, dto.FlatNumber), dto.ZipCode));

            var address = await _addressService.AddAsync(dto);

            address.ShouldNotBeNull();
            address.CountryName.ShouldBe(dto.CountryName);
            address.CityName.ShouldBe(dto.CityName);
            address.StreetName.ShouldBe(dto.StreetName);
            address.BuildingNumber.ShouldBe(dto.BuildingNumber);
            address.FlatNumber.ShouldBe(dto.FlatNumber);
            address.ZipCode.ShouldBe(dto.ZipCode);
        }

        [Theory]
        [InlineData("Poland", "Zielona Góra", "Wrocławska", 10, 1, "65-510")]
        [InlineData("Poland", "Zielona Góra", "Szafrana", 20, null, "65-520")]
        public async Task should_update_address(string countryName, string cityName, string streetName, int buildingNumber, int? flatNumber, string zipCode)
        {
            var address = AddDefaultAddress();
            var dto = new AddressDto(address.Id, streetName, cityName, countryName, buildingNumber, zipCode, flatNumber);
            _addressRepository.UpdateAsync(Arg.Any<Address>()).Returns(new Address(address.Id, AddressLocation.From(dto.CountryName, dto.CityName, dto.StreetName, dto.BuildingNumber, dto.FlatNumber), dto.ZipCode));

            var addressDto = await _addressService.UpdateAsync(dto);

            addressDto.ShouldNotBeNull();
            addressDto.CountryName.ShouldBe(dto.CountryName);
            addressDto.CityName.ShouldBe(dto.CityName);
            addressDto.StreetName.ShouldBe(dto.StreetName);
            addressDto.BuildingNumber.ShouldBe(dto.BuildingNumber);
            addressDto.FlatNumber.ShouldBe(dto.FlatNumber);
            addressDto.ZipCode.ShouldBe(dto.ZipCode);
        }

        [Fact]
        public async Task given_not_existing_address_when_update_should_throw_an_exception()
        {
            var dto = new AddressDto(1, "streetName", "cityName", "countryName", 1, "zipCode", 1);

            var exception = await Record.ExceptionAsync(() => _addressService.UpdateAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task should_delete_address()
        {
            var address = AddDefaultAddress();

            await _addressService.DeleteAsync(address.Id);

            await _addressRepository.Received(1).DeleteAsync(address);
        }

        [Fact]
        public async Task given_not_existing_address_when_delete_should_throw_an_exception()
        {
            var id = 1;

            var exception = await Record.ExceptionAsync(() => _addressService.DeleteAsync(id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task given_address_used_by_customer_when_delete_should_throw_an_exception()
        {
            var address = AddDefaultAddressWithCustomer();

            var exception = await Record.ExceptionAsync(() => _addressService.DeleteAsync(address.Id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Cannot delete Address with id");
        }

        private Address AddDefaultAddress(int? id = null)
        {
            var address = EntitiesFixtures.CreateDefaultAddress(id);
            _addressRepository.GetAsync(address.Id).Returns(address);
            return address;
        }

        private Address AddDefaultAddressWithCustomer(int? id = null)
        {
            var address = EntitiesFixtures.CreateDefaultAddressWithCustomer(id);
            _addressRepository.GetAsync(address.Id).Returns(address);
            return address;
        }

        private readonly IAddressService _addressService;
        private readonly IAddressRepository _addressRepository;

        public AddressServiceTests()
        {
            _addressRepository = Substitute.For<IAddressRepository>();
            _addressService = new AddressService(_addressRepository);
        }
    }
}
