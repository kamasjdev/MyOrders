using Microsoft.Extensions.Configuration;
using MyOrders.Infrastructure;

namespace MyOrders.IntegrationTests.Common
{
    public sealed class OptionsProvider
    {
        private readonly IConfigurationRoot _configuration;

        public OptionsProvider()
        {
            _configuration = GetConfigurationRoot();
        }

        public T Get<T>(string sectionName) where T : class, new() => _configuration.GetOptions<T>(sectionName);

        private static IConfigurationRoot GetConfigurationRoot()
            => new ConfigurationBuilder()
                .AddJsonFile("appsettings.tests.json", true)
                .AddEnvironmentVariables()
                .Build();
    }
}
