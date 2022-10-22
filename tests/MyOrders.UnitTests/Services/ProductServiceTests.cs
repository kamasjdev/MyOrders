using MyOrders.Application.Abstractions;
using MyOrders.Application.DTO;
using MyOrders.Application.Exceptions;
using MyOrders.Application.Mappings;
using MyOrders.Application.Services;
using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;
using MyOrders.UnitTests.Fixtures;
using NSubstitute;
using Shouldly;
using System.Threading.Tasks;

namespace MyOrders.UnitTests.Services
{
    public class ProductServiceTests
    {
        [Fact]
        public async Task should_create_product()
        {
            var productKind = AddDefaultProductKind();
            var dto = new ProductDetailsDto(0, "Product#1", new ProductKindDto(productKind.Id, ""), 100M);
            _productRepository.AddAsync(Arg.Any<Product>()).Returns(new Product(1, dto.ProductName, productKind, dto.Price));

            var product = await _productService.AddAsync(dto);

            product.ShouldNotBeNull();
            product.ProductName.ShouldBe(dto.ProductName);
            product.ProductKind.ShouldNotBeNull();
            product.ProductKind.Id.ShouldBe(dto.ProductKind.Id);
            product.Price.ShouldBe(dto.Price);
        }

        [Fact]
        public async Task given_invalid_product_kind_when_add_should_throw_an_exception()
        {
            var dto = new ProductDetailsDto(0, "Prod", null, 100M);

            var exception = await Record.ExceptionAsync(() => _productService.AddAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldBe("Product cannot contain null ProductKind");
        }

        [Fact]
        public async Task given_existing_product_name_when_add_should_throw_an_exception()
        {
            var product = AddDefaultProduct();
            var productKind = AddDefaultProductKind();
            var dto = new ProductDetailsDto(0, product.ProductName, productKind.AsDto(), 100M);

            var exception = await Record.ExceptionAsync(() => _productService.AddAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Product with ProductName");
            exception.Message.ShouldContain("already exists");
        }

        [Fact]
        public async Task given_not_existing_product_kind_when_add_should_throw_an_exception()
        {
            var dto = new ProductDetailsDto(0, "ProductName", new ProductKindDto(1, ""), 100M);

            var exception = await Record.ExceptionAsync(() => _productService.AddAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task should_update_product()
        {
            var product = AddDefaultProduct();
            var productKind = AddDefaultProductKind(2);
            var dto = new ProductDetailsDto(product.Id, "test-123", productKind.AsDto(), 200M);
            _productRepository.UpdateAsync(Arg.Any<Product>()).Returns(new Product(dto.Id, dto.ProductName, productKind, dto.Price));

            var productUpdated = await _productService.UpdateAsync(dto);

            productUpdated.ShouldNotBeNull();
            productUpdated.ProductName.ShouldBe(dto.ProductName);
            productUpdated.ProductKind.ShouldNotBeNull();
            productUpdated.ProductKind.Id.ShouldBe(dto.ProductKind.Id);
            productUpdated.Price.ShouldBe(dto.Price);
        }

        [Fact]
        public async Task given_invalid_product_kind_when_update_should_throw_an_exception()
        {
            var dto = new ProductDetailsDto(0, "Prod", null, 100M);

            var exception = await Record.ExceptionAsync(() => _productService.UpdateAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldBe("Product cannot contain null ProductKind");
        }

        [Fact]
        public async Task given_existing_product_name_when_update_should_throw_an_exception()
        {
            var product = AddDefaultProduct();
            var productKind = AddDefaultProductKind();
            var dto = new ProductDetailsDto(0, product.ProductName, productKind.AsDto(), 100M);

            var exception = await Record.ExceptionAsync(() => _productService.UpdateAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Product with ProductName");
            exception.Message.ShouldContain("already exists");
        }

        [Fact]
        public async Task given_not_existing_product_kind_when_update_should_throw_an_exception()
        {
            var dto = new ProductDetailsDto(0, "ProductName", new ProductKindDto(1, ""), 100M);

            var exception = await Record.ExceptionAsync(() => _productService.UpdateAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("ProductKind");
            exception.Message.ShouldContain("was not found");
        }   
        
        [Fact]
        public async Task given_not_existing_product_when_update_should_throw_an_exception()
        {
            var productKind = AddDefaultProductKind();
            var dto = new ProductDetailsDto(0, "ProductName", productKind.AsDto(), 100M);

            var exception = await Record.ExceptionAsync(() => _productService.UpdateAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Product");
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task should_delete_product()
        {
            var product = AddDefaultProduct();

            await _productService.DeleteAsync(product.Id);

            await _productRepository.Received(1).DeleteAsync(product);
        }

        [Fact]
        public async Task given_not_existing_product_when_delete_should_throw_an_exception()
        {
            var id = 1;

            var exception = await Record.ExceptionAsync(() => _productService.DeleteAsync(id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("Product");
            exception.Message.ShouldContain("was not found");
        }

        private ProductKind AddDefaultProductKind(int? id = null)
        {
            var productKind = EntitiesFixtures.CreateDefaultProductKind(id);
            _productKindRepository.GetAsync(productKind.Id).Returns(productKind);
            return productKind;
        }

        private Product AddDefaultProduct(int? id = null)
        {
            var product = EntitiesFixtures.CreateDefaultProduct(id);
            _productRepository.GetAsync(product.Id).Returns(product);
            _productRepository.ExistsByProductNameAsync(product.ProductName).Returns(true);
            return product;
        }

        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;
        private readonly IProductKindRepository _productKindRepository;

        public ProductServiceTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _productKindRepository = Substitute.For<IProductKindRepository>();
            _productService = new ProductService(_productRepository, _productKindRepository);
        }
    }
}
