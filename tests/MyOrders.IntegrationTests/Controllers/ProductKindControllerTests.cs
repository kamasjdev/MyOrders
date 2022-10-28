using Flurl.Http;
using Microsoft.AspNetCore.Http;
using MyOrders.Application.DTO;
using MyOrders.IntegrationTests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MyOrders.IntegrationTests.Controllers
{
    public class ProductKindControllerTests : BaseControllerTest
    {
        [Fact]
        public async Task should_add_product_kind_and_return_201()
        {
            var dto = new ProductKindDto(0, "Name");

            var response = await Client.Request(Path).PostJsonAsync(dto);

            response.StatusCode.ShouldBe(StatusCodes.Status201Created);
            var productKindAdded = await response.ResponseMessage.Content.ReadFromJsonAsync<ProductKindDto>();
            productKindAdded.ProductKindName.ShouldBe(dto.ProductKindName);
        }

        [Fact]
        public async Task should_update_product_kind_and_return_200()
        {
            var addDto = new ProductKindDto(0, "Name");
            var response = await Client.Request(Path).PostJsonAsync(addDto);
            var productKindAdded = await response.ResponseMessage.Content.ReadFromJsonAsync<ProductKindDto>();
            var dto = new ProductKindDto(productKindAdded.Id, "ABCD");

            var productKindUpdatedResponse = await Client.Request($"{Path}/{productKindAdded.Id}").PutJsonAsync(dto);

            productKindUpdatedResponse.StatusCode.ShouldBe(StatusCodes.Status200OK);
            var productKindUpdated = await productKindUpdatedResponse.ResponseMessage.Content.ReadFromJsonAsync<ProductKindDto>();
            productKindUpdated.ProductKindName.ShouldBe(dto.ProductKindName);
        }

        [Fact]
        public async Task should_delete_product_kind_return_204_and_delete_from_db()
        {
            var productKind = await AddDefaultProductKind();

            var response = await Client.Request($"{Path}/{productKind.Id}").DeleteAsync();

            response.StatusCode.ShouldBe(StatusCodes.Status204NoContent);
            var responseAfterDelete = await Client.Request($"{Path}/{productKind.Id}").AllowHttpStatus("404").GetAsync();
            responseAfterDelete.StatusCode.ShouldBe(StatusCodes.Status404NotFound);

        }

        [Fact]
        public async Task should_get_product_kind()
        {
            var productKind = await AddDefaultProductKind();

            var response = await Client.Request($"{Path}/{productKind.Id}").GetAsync();

            response.StatusCode.ShouldBe(StatusCodes.Status200OK);
            var dto = await response.ResponseMessage.Content.ReadFromJsonAsync<ProductKindDto>();
            dto.ShouldNotBeNull();
            dto.ProductKindName.ShouldBe(productKind.ProductKindName);
        }

        [Fact]
        public async Task should_get_product_kinds()
        {
            await AddDefaultProductKind();
            await AddDefaultProductKind();
            await AddDefaultProductKind();

            var response = await Client.Request($"{Path}").GetAsync();

            response.StatusCode.ShouldBe(StatusCodes.Status200OK);
            var dtos = await response.ResponseMessage.Content.ReadFromJsonAsync<IEnumerable<ProductKindDto>>();
            dtos.ShouldNotBeEmpty();
            dtos.Count().ShouldBe(3);
        }

        public async Task<ProductKindDto> AddDefaultProductKind()
        {
            var dto = new ProductKindDto(0, $"Name{Guid.NewGuid()}");
            var response = await Client.Request(Path).PostJsonAsync(dto);
            response.StatusCode.ShouldBe(StatusCodes.Status201Created);
            var productKindAdded = await response.ResponseMessage.Content.ReadFromJsonAsync<ProductKindDto>();
            return productKindAdded;
        }

        private const string Path = "api/product-kinds";

        public ProductKindControllerTests(OptionsProvider optionsProvider) : base(optionsProvider)
        { }
    }
}
