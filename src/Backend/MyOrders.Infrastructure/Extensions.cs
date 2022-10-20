using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyOrders.Infrastructure.DAL;
using MyOrders.Infrastructure.Exceptions;
using MyOrders.Infrastructure.Time;

namespace MyOrders.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTime();
            services.AddInMemoryRepositories();
            services.AddExceptionHandling();
            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseExceptionHandling();
            return app;
        }
    }
}