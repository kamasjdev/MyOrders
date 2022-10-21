using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyOrders.Infrastructure.DAL.Initlializer
{
    internal sealed class DatabaseInitializer : IHostedService
    {
        private readonly ILogger<DatabaseInitializer> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DatabaseInitializer(ILogger<DatabaseInitializer> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MyOrdersDbContext>();
            _logger.LogInformation("Initialize database, Prepare for migrations");
            await dbContext.Database.MigrateAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
