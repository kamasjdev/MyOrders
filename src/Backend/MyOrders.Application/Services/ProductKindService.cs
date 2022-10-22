using MyOrders.Application.Abstractions;
using MyOrders.Application.DTO;
using MyOrders.Application.Exceptions;
using MyOrders.Application.Mappings;
using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;

namespace MyOrders.Application.Services
{
    internal sealed class ProductKindService : IProductKindService
    {
        private readonly IProductKindRepository _productKindRepository;

        public ProductKindService(IProductKindRepository productKindRepository)
        {
            _productKindRepository = productKindRepository;
        }

        public async Task<ProductKindDto> AddAsync(ProductKindDto productKindDto)
        {
            await CheckProductKindName(productKindDto.ProductKindName);
            var productKind = ProductKind.Create(productKindDto.ProductKindName);
            return (await _productKindRepository.AddAsync(productKind)).AsDto();
        }

        public async Task DeleteAsync(int id)
        {
            var productKind = await GetProductKindAsync(id);
            await _productKindRepository.DeleteAsync(productKind);
        }

        public async Task<IEnumerable<ProductKindDto>> GetAllAsync()
        {
            return (await _productKindRepository.GetAllAsync()).Select(pk => pk.AsDto());
        }

        public async Task<ProductKindDto> GetAsync(int id)
        {
            return (await _productKindRepository.GetAsync(id))?.AsDto();
        }

        public async Task<ProductKindDto> UpdateAsync(ProductKindDto productKindDto)
        {
            var productKind = await GetProductKindAsync(productKindDto.Id);
            await CheckProductKindName(productKindDto.ProductKindName);
            productKind.ChangeProductKindName(productKindDto.ProductKindName);
            return (await _productKindRepository.UpdateAsync(productKind)).AsDto();
        }

        private async Task<ProductKind> GetProductKindAsync(int id)
        {
            var productKind = await _productKindRepository.GetAsync(id);

            if (productKind is null)
            {
                throw new BusinessException($"ProductKind with id: '{id}' was not found");
            }

            return productKind;
        }

        private async Task CheckProductKindName(string productKindName)
        {
            var exists = await _productKindRepository.ExistsByProductKindNameAsync(productKindName);

            if (exists)
            {
                throw new BusinessException($"ProductKind with ProductKindName: '{productKindName}' already exists");
            }
        }
    }
}
