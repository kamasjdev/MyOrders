using Flurl.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyOrders.Infrastructure.DAL;
using System;

namespace MyOrders.IntegrationTests.Common
{
    [Collection("api")]
    public abstract class BaseControllerTest : IClassFixture<OptionsProvider>
    {
        protected IFlurlClient Client { get; }
        protected OptionsProvider OptionsProvider { get; }

        public BaseControllerTest(OptionsProvider optionsProvider)
        {
            OptionsProvider = optionsProvider;
            var app = new TestAppFactory(ConfigureServices);
            Client = new FlurlClient(app.Client);
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            var servicesProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            services.AddDbContext<MyOrdersDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDatabase");
                options.UseInternalServiceProvider(servicesProvider);
            });

            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            EnsureCreateDatabase(scope.ServiceProvider);
        }

        private void EnsureCreateDatabase(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<MyOrdersDbContext>();
            var logger = serviceProvider.GetRequiredService<ILogger<TestAppFactory>>();
            logger.LogInformation("Creating database");
            context.Database.EnsureCreated();
        }
    }
}
