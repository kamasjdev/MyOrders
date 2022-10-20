using Microsoft.Extensions.DependencyInjection;
using MyOrders.Application.Abstractions;
using MyOrders.Application.Services;

namespace MyOrders.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductKindService, ProductKindService>();
            return services;
        }
    }
}