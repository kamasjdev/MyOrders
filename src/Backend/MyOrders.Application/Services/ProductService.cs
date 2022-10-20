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

        public async Task<ProductDto> AddAsync(ProductDto productDto)
        {
            if (productDto.ProductKind is null)
            {
                throw new BusinessException("Product cannot contain null ProductKind");
            }

            var productKind = await _productKindRepository.GetAsync(productDto.ProductKind.Id);

            if (productKind is null)
            {
                throw new BusinessException($"ProductKind with id: '{productDto.ProductKind.Id}' was not found");
            }

            var product = Product.Create(productDto.ProductName, productKind, productDto.Price);
            return (await _productRepository.AddAsync(product)).AsDto();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);

            if (product is null)
            {
                throw new BusinessException($"Product with id: '{id}' was not found");
            }

            await _productRepository.DeleteAsync(product);
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            return (await _productRepository.GetAllAsync()).Select(p => p.AsDto());
        }

        public async Task<ProductDto> GetAsync(int id)
        {
            return (await _productRepository.GetAsync(id))?.AsDto();
        }

        public async Task<ProductDto> UpdateAsync(ProductDto productDto)
        {
            if (productDto.ProductKind is null)
            {
                throw new BusinessException("Product cannot contain null ProductKind");
            }

            var productKind = await _productKindRepository.GetAsync(productDto.ProductKind.Id);

            if (productKind is null)
            {
                throw new BusinessException($"ProductKind with id: '{productDto.ProductKind.Id}' was not found");
            }

            var product = await _productRepository.GetAsync(productDto.Id);
            product.ChangeProductName(productDto.ProductName);
            product.ChangeProductKind(productKind);
            product.ChangePrice(product.Price);

            return (await _productRepository.UpdateAsync(product)).AsDto();
        }
    }
}
