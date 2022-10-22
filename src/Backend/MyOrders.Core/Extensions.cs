using Microsoft.Extensions.DependencyInjection;
using MyOrders.Core.Services;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MyOrders.UnitTests")]
namespace MyOrders.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddSingleton<IOrderNumberGenerator, OrderNumberGenerator>();
            return services;
        }
    }
}