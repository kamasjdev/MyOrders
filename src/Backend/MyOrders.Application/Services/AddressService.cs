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
            var address = Address.Create(AddressLocation.From(addressDto.CountryName, addressDto.CityName, addressDto.StreetName,
                                addressDto.BuildingNumber, addressDto.FlatNumber), addressDto.ZipCode);
            return (await _addressRepository.AddAsync(address)).AsDto();
        }

        public async Task DeleteAsync(int id)
        {
            var address = await GetAddressAsync(id);
            
            if (address.Customer is not null)
            {
                throw new BusinessException($"Cannot delete Address with id: '{address.Id}', because is used by Customer with id: '{address.Customer.Id}'");
            }

            await _addressRepository.DeleteAsync(address);
        }

        public async Task<AddressDto> GetAsync(int id)
        {
            return (await _addressRepository.GetAsync(id))?.AsDto();
        }

        public async Task<AddressDto> UpdateAsync(AddressDto addressDto)
        {
            var address = await GetAddressAsync(addressDto.Id);
            address.ChangeAddressLocation(AddressLocation.From(addressDto.CountryName, addressDto.CityName, addressDto.StreetName, addressDto.BuildingNumber, addressDto.FlatNumber));
            address.ChangeZipCode(addressDto.ZipCode);
            return (await _addressRepository.UpdateAsync(address)).AsDto();
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
