using Microsoft.EntityFrameworkCore;

namespace MyOrders.Infrastructure.DAL
{
    internal sealed class MyOrdersDbContext : DbContext
    {
        public MyOrdersDbContext(DbContextOptions<MyOrdersDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
