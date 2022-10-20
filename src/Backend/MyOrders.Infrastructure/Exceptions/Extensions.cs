using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MyOrders.Infrastructure.Exceptions
{
    internal static class Extensions
    {
        public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
        {
            services.AddSingleton<ErrorHandlerMiddleware>();
            return services;
        }

        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlerMiddleware>();
            return app;
        }
    }
}
