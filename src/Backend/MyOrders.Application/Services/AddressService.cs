using MyOrders.Application.Abstractions;
using MyOrders.Application.DTO;
using MyOrders.Application.Exceptions;
using MyOrders.Application.Mappings;
using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;
using MyOrders.Core.ValueObjects;

namespace MyOrders.Application.Services
{
    internal sealed class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<AddressDto> AddAsync(AddressDto addressDto)
        {
            Address address;
            if (addressDto.FlatNumber.HasValue)
            {
                address = Address.Create(AddressLocation.From(addressDto.CountryName, addressDto.CityName, addressDto.StreetName,
                            addressDto.BuildingNumber, addressDto.FlatNumber.Value));
            }
            else
            {
                address = Address.Create(AddressLocation.From(addressDto.CountryName, addressDto.CityName, addressDto.StreetName,
                                addressDto.BuildingNumber));
            }

            return (await _addressRepository.AddAsync(address)).AsDto();
        }

        public async Task DeleteAsync(int id)
        {
            var address = await GetAddressAsync(id);
            await _addressRepository.DeleteAsync(address);
        }

        public async Task<AddressDto> GetAsync(int id)
        {
            return (await _addressRepository.GetAsync(id))?.AsDto();
        }

        public async Task<AddressDto> UpdateAsync(AddressDto addressDto)
        {
            var address = await GetAddressAsync(addressDto.Id);

            if (addressDto.FlatNumber.HasValue)
            {
                address.ChangeAddressLocation(AddressLocation.From(addressDto.CountryName, addressDto.CityName, addressDto.StreetName, addressDto.BuildingNumber, addressDto.FlatNumber.Value));
                return (await _addressRepository.UpdateAsync(address)).AsDto();
            }
            else
            {
                address.ChangeAddressLocation(AddressLocation.From(addressDto.CountryName, addressDto.CityName, addressDto.StreetName, addressDto.BuildingNumber));
                return (await _addressRepository.UpdateAsync(address)).AsDto();
            }
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
    }
}
