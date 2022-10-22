using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyOrders.Core.Entities;
using MyOrders.Core.ValueObjects;

namespace MyOrders.Infrastructure.DAL.Configurations
{
    internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .UseMySqlIdentityColumn();

            builder.Property(p => p.ProductName)
                .HasConversion(p => p.Value, mail => new ProductName(mail))
                .IsRequired()
                .HasMaxLength(200);
            builder.HasIndex(p => p.ProductName).IsUnique();

            builder.Property(p => p.Price)
                .HasConversion(c => c.Value, price => new Price(price))
                .IsRequired()
                .HasPrecision(14, 4);
        }
    }
}
