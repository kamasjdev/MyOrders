using MyOrders.Application.Abstractions;
using MyOrders.Application.DTO;
using MyOrders.Application.Exceptions;
using MyOrders.Application.Services;
using MyOrders.Core.Entities;
using MyOrders.Core.Repositories;
using MyOrders.UnitTests.Fixtures;
using NSubstitute;
using Shouldly;
using System.Threading.Tasks;

namespace MyOrders.UnitTests.Services
{
    public class ProductKindServiceTests
    {
        [Fact]
        public async Task should_add_product_kind()
        {
            var dto = new ProductKindDto(0, "Kind#1");
            _productKindRepository.AddAsync(Arg.Any<ProductKind>()).Returns(new ProductKind(1, dto.ProductKindName));

            var productKind = await _productKindService.AddAsync(dto);

            productKind.ShouldNotBeNull();
            productKind.ProductKindName.ShouldBe(dto.ProductKindName);
        }

        [Fact]
        public async Task should_update_product_kind()
        {
            var productKind = AddDefaultProductKind();
            var dto = new ProductKindDto(productKind.Id, "Kind#ab1");
            _productKindRepository.UpdateAsync(Arg.Any<ProductKind>()).Returns(new ProductKind(dto.Id, dto.ProductKindName));

            var productKindUpdated = await _productKindService.UpdateAsync(dto);

            productKindUpdated.ShouldNotBeNull();
            productKindUpdated.ProductKindName.ShouldBe(dto.ProductKindName);
        }

        [Fact]
        public async Task given_not_existing_product_kind_when_update_should_throw_an_exception()
        {
            var dto = new ProductKindDto(1, "Kind#ab1");

            var exception = await Record.ExceptionAsync(() => _productKindService.UpdateAsync(dto));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("was not found");
        }

        [Fact]
        public async Task should_delete_product_kind()
        {
            var contactData = AddDefaultProductKind();

            await _productKindService.DeleteAsync(contactData.Id);

            await _productKindRepository.Received(1).DeleteAsync(contactData);
        }

        [Fact]
        public async Task given_not_existing_product_kind_when_delete_shold_throw_an_exception()
        {
            var id = 1;

            var exception = await Record.ExceptionAsync(() => _productKindService.DeleteAsync(id));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<BusinessException>();
            exception.Message.ShouldNotBeNullOrWhiteSpace();
            exception.Message.ShouldContain("was not found");
        }

        private ProductKind AddDefaultProductKind(int? id = null)
        {
            var productKind = EntitiesFixtures.CreateDefaultProductKind(id);
            _productKindRepository.GetAsync(productKind.Id).Returns(productKind);
            return productKind;
        }

        private readonly IProductKindService _productKindService;
        private readonly IProductKindRepository _productKindRepository;

        public ProductKindServiceTests()
        {
            _productKindRepository = Substitute.For<IProductKindRepository>();
            _productKindService = new ProductKindService(_productKindRepository);
        }
    }
}
