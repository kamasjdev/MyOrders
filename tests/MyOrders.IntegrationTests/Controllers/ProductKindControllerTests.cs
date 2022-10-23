using Flurl.Http;
using Microsoft.AspNetCore.Http;
using MyOrders.Application.DTO;
using MyOrders.IntegrationTests.Common;
using Shouldly;
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

        private const string Path = "api/product-kinds";

        public ProductKindControllerTests(OptionsProvider optionsProvider) : base(optionsProvider)
        { }
    }
}
