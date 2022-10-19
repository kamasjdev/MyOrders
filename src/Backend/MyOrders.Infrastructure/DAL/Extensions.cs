using Microsoft.Extensions.DependencyInjection;
using MyOrders.Core.Repositories;
using MyOrders.Infrastructure.DAL.Repositories.InMemory;

namespace MyOrders.Infrastructure.DAL
{
    internal static class Extensions
    {
        public static IServiceCollection AddInMemoryRepositories(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IInMemoryRepository<>), typeof(InMemoryRepository<>));
            services.AddSingleton<IAddressRepository, InMemoryAddressRepostiory>();
            services.AddSingleton<IContactDataRepository, InMemoryContactDataRepository>();
            services.AddSingleton<ICustomerRepository, InMemoryCustomerRepository>();
            services.AddSingleton<IOrderItemRepository, InMemoryOrderItemRepository>();
            services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
            services.AddSingleton<IProductKindRepository, InMemoryProductKindRepository>();
            services.AddSingleton<IProductRepository, InMemoryProductRepository>();
            return services;
        }
    }
}
