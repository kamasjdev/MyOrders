using Flurl.Http;
using MyOrders.Application.DTO;
using MyOrders.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MyOrders.IntegrationTests.Controllers
{
    public class ProductController : BaseControllerTest, IAsyncLifetime
    {
        [Fact]
        public async Task should_add_product_and_return_201()
        {
            var productKindDto = _productKinds.First();
            var dto = new ProductDetailsDto(0, "Name", productKindDto, 120);

            var response = await Client.Request(Path).PostJsonAsync(dto);

            response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
            var productAdded = await response.ResponseMessage.Content.ReadFromJsonAsync<ProductDetailsDto>();
            productAdded.ProductName.ShouldBe(dto.ProductName);
            productAdded.Price.ShouldBe(dto.Price);
            productAdded.ProductKind.Id.ShouldBe(dto.ProductKind.Id);
            productAdded.ProductKind.ProductKindName.ShouldBe(dto.ProductKind.ProductKindName);
        }

        [Fact]
        public async Task should_update_product_and_return_200()
        {
            var productAdded = await AddDefaultProduct();
            var dto = new ProductDetailsDto(productAdded.Id, "ABCDaef", _productKinds.OrderByDescending(pk => pk.Id).First(), 99);

            var productKindUpdatedResponse = await Client.Request($"{Path}/{productAdded.Id}").PutJsonAsync(dto);

            productKindUpdatedResponse.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var productUpdated = await productKindUpdatedResponse.ResponseMessage.Content.ReadFromJsonAsync<ProductDetailsDto>();
            productUpdated.ProductName.ShouldBe(dto.ProductName);
            productUpdated.Price.ShouldBe(dto.Price);
            productUpdated.ProductKind.Id.ShouldBe(dto.ProductKind.Id);
            productUpdated.ProductKind.ProductKindName.ShouldBe(dto.ProductKind.ProductKindName);
        }

        [Fact]
        public async Task should_delete_product_kind_return_204_and_delete_from_db()
        {
            var product = await AddDefaultProduct();

            var response = await Client.Request($"{Path}/{product.Id}").DeleteAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.NoContent);
            var responseAfterDelete = await Client.Request($"{Path}/{product.Id}").AllowHttpStatus("404").GetAsync();
            responseAfterDelete.StatusCode.ShouldBe((int)HttpStatusCode.NotFound);

        }

        [Fact]
        public async Task should_get_product()
        {
            var product = await AddDefaultProduct();

            var response = await Client.Request($"{Path}/{product.Id}").GetAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var dto = await response.ResponseMessage.Content.ReadFromJsonAsync<ProductDetailsDto>();
            dto.ShouldNotBeNull();
            dto.ProductName.ShouldBe(product.ProductName);
        }

        [Fact]
        public async Task should_get_products()
        {
            await AddDefaultProduct();
            await AddDefaultProduct();
            await AddDefaultProduct();

            var response = await Client.Request($"{Path}").GetAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var dtos = await response.ResponseMessage.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
            dtos.ShouldNotBeEmpty();
            dtos.Count().ShouldBe(3);
        }

        public async Task<ProductDetailsDto> AddDefaultProduct()
        {
            var productKindDto = _productKinds.First();
            var dto = new ProductDetailsDto(0, $"Name{Guid.NewGuid():N}", productKindDto, 100);
            var response = await Client.Request(Path).PostJsonAsync(dto);
            response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
            var productAdded = await response.ResponseMessage.Content.ReadFromJsonAsync<ProductDetailsDto>();
            return productAdded;
        }

        public async Task<ProductKindDto> AddDefaultProductKind()
        {
            var dto = new ProductKindDto(0, $"Name{Guid.NewGuid()}");
            var response = await Client.Request("api/product-kinds").PostJsonAsync(dto);
            response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
            var productKindAdded = await response.ResponseMessage.Content.ReadFromJsonAsync<ProductKindDto>();
            return productKindAdded;
        }

        public async Task InitializeAsync()
        {
            _productKinds.Add(await AddDefaultProductKind());
            _productKinds.Add(await AddDefaultProductKind());
            _productKinds.Add(await AddDefaultProductKind());
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        private List<ProductKindDto> _productKinds = new();
        private const string Path = "api/products";

        public ProductController(OptionsProvider optionsProvider) : base(optionsProvider)
        { }
    }
}
