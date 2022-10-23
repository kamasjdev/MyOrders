using Flurl.Http;
using MyOrders.Infrastructure.App;
using MyOrders.IntegrationTests.Common;
using Shouldly;
using System.Net;
using System.Threading.Tasks;

namespace MyOrders.IntegrationTests.Controllers
{
    public class HealthCheckControllerTests : BaseControllerTest
    {
        [Fact]
        public async Task should_return_app_name()
        {
            var appOptions = OptionsProvider.Get<AppOptions>("app");

            var response = await Client.Request("api").GetAsync();

            response.StatusCode.ShouldBe((int)HttpStatusCode.OK);
            var content = await response.ResponseMessage.Content.ReadAsStringAsync();
            content.ShouldBe(appOptions.Name);
        }

        public HealthCheckControllerTests(OptionsProvider optionsProvider) : base(optionsProvider)
        { }
    }
}
