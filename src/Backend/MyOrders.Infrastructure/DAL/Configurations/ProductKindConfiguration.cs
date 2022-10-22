using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyOrders.Core.Entities;
using MyOrders.Core.ValueObjects;

namespace MyOrders.Infrastructure.DAL.Configurations
{
    internal sealed class ProductKindConfiguration : IEntityTypeConfiguration<ProductKind>
    {
        public void Configure(EntityTypeBuilder<ProductKind> builder)
        {
            builder.HasKey(pk => pk.Id);
            builder.Property(pk => pk.Id)
                .UseMySqlIdentityColumn();

            builder.Property(pk => pk.ProductKindName)
                .HasConversion(pk => pk.Value, name => new ProductKindName(name))
                .IsRequired()
                .HasMaxLength(150);
            builder.HasIndex(p => p.ProductKindName).IsUnique();

            builder.HasMany(pk => pk.Products).WithOne(p => p.ProductKind);
        }
    }
}
