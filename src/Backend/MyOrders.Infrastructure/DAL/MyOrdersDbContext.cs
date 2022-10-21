using Microsoft.EntityFrameworkCore;
using MyOrders.Core.Entities;

namespace MyOrders.Infrastructure.DAL
{
    internal sealed class MyOrdersDbContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<ContactData> ContactDatas { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductKind> ProductKinds { get; set; }
        public DbSet<Product> Products { get; set; }

        public MyOrdersDbContext(DbContextOptions<MyOrdersDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
