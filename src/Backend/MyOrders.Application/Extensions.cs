using Microsoft.Extensions.DependencyInjection;
using MyOrders.Application.Abstractions;
using MyOrders.Application.Services;

namespace MyOrders.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IContactDataService, ContactDataService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductKindService, ProductKindService>();
            return services;
        }
    }
}