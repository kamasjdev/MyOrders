using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace MyOrders.IntegrationTests.Common
{
    internal sealed class TestAppFactory : WebApplicationFactory<Program>
    {
        public HttpClient Client { get; }

        public TestAppFactory(Action<IServiceCollection> services = null)
        {
            Client = WithWebHostBuilder(builder =>
            {
                if (services is not null)
                {
                    builder.ConfigureServices(services);
                }

                builder.UseEnvironment("tests");
            }).CreateClient();
        }
    }
}
