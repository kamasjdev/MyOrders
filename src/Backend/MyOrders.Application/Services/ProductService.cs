using MyOrders.Application.Abstractions;
using MyOrders.Application.DTO;
using MyOrders.Application.Exceptions;
using MyOrders.Application.Mappings;
using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;

namespace MyOrders.Application.Services
{
    internal sealed class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductKindRepository _productKindRepository;

        public ProductService(IProductRepository productRepository, IProductKindRepository productKindRepository)
        {
            _productRepository = productRepository;
            _productKindRepository = productKindRepository;
        }

        public async Task<ProductDetailsDto> AddAsync(ProductDetailsDto productDto)
        {
            CheckProductKind(productDto.ProductKind);
            var productKind = await GetProductKindAsync(productDto.ProductKind.Id);
            var product = Product.Create(productDto.ProductName, productKind, productDto.Price);
            return (await _productRepository.AddAsync(product)).AsDetailsDto();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetProductAsync(id);
            await _productRepository.DeleteAsync(product);
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            return (await _productRepository.GetAllAsync()).Select(p => p.AsDto());
        }

        public async Task<ProductDetailsDto> GetAsync(int id)
        {
            return (await _productRepository.GetAsync(id))?.AsDetailsDto();
        }

        public async Task<ProductDetailsDto> UpdateAsync(ProductDetailsDto productDto)
        {
            CheckProductKind(productDto.ProductKind);
            var productKind = await GetProductKindAsync(productDto.ProductKind.Id);
            var product = await GetProductAsync(productDto.Id);
            product.ChangeProductName(productDto.ProductName);
            product.ChangeProductKind(productKind);
            product.ChangePrice(productDto.Price);

            return (await _productRepository.UpdateAsync(product)).AsDetailsDto();
        }

        private void CheckProductKind(ProductKindDto productKindDto)
        {
            if (productKindDto is null)
            {
                throw new BusinessException("Product cannot contain null ProductKind");
            }
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

        private async Task<Product> GetProductAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);

            if (product is null)
            {
                throw new BusinessException($"Product with id: '{id}' was not found");
            }

            return product;
        }
    }
}
