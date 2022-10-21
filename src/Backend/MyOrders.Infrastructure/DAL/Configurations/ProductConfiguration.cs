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
                .HasConversion(p => p.Value, id => new EntityId(id));

            builder.OwnsOne(p => p.ProductName, navigation =>
            {
                navigation.Property(pn => pn.Value).HasColumnName(nameof(Product.ProductName)).IsRequired();
            });

            builder.OwnsOne(p => p.Price, navigation =>
            {
                navigation.Property(pr => pr.Value).HasColumnName(nameof(Product.Price)).IsRequired();
            });

            builder.HasIndex(p => p.ProductName).IsUnique();
        }
    }
}
