using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
            _logger.LogInformation("Initialize database, Prepare for migrations");
            var database = scope.ServiceProvider.GetRequiredService<IOptionsMonitor<Database>>().CurrentValue;

            if (database.DatabaseKind is DatabaseKind.InMemory or DatabaseKind.EFCoreInMemory)
            {
                _logger.LogInformation($"Database {database.DatabaseKind} is non relational, so the migration process is skipped");
                return;
            }

            var dbContext = scope.ServiceProvider.GetRequiredService<MyOrdersDbContext>();
            await dbContext.Database.MigrateAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
