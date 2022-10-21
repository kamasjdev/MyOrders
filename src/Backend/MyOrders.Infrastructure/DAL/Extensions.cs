using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyOrders.Core.Repositories;
using MyOrders.Infrastructure.DAL.Repositories.InMemory;

namespace MyOrders.Infrastructure.DAL
{
    internal static class Extensions
    {
        public static IServiceCollection AddMySqlOptions(this IServiceCollection services)
        {
            var options = services.GetOptions<MySqlOptions>("mysql");

            services.Configure<MySqlOptions>(config =>
            {
                config.ConnectionString = options.ConnectionString;
            });

            return services;
        }

        public static IServiceCollection AddMySql<T>(this IServiceCollection services)
            where T : DbContext
        {
            var options = services.GetOptions<MySqlOptions>("mysql");
            ServerVersion serverVersion = ServerVersion.AutoDetect(options.ConnectionString);
            services.AddDbContext<T>(context => context.UseMySql(options.ConnectionString, serverVersion));
            return services;
        }

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
