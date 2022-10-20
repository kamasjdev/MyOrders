using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyOrders.Infrastructure.App;
using MyOrders.Infrastructure.Conventions;
using MyOrders.Infrastructure.DAL;
using MyOrders.Infrastructure.Exceptions;
using MyOrders.Infrastructure.Time;

namespace MyOrders.Infrastructure
{
    public static class Extensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppOptions>(configuration.GetRequiredSection("app"));
            services.AddControllers(options => options.UseDashedConventionInRouting());
            services.AddTime();
            services.AddInMemoryRepositories();
            services.AddExceptionHandling();
            services.AddSwaggerGen();
            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        {
            app.UseExceptionHandling();
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}