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
                .HasConversion(pk => pk.Value, id => new EntityId(id));

            builder.OwnsOne(pk => pk.ProductKindName, navigation =>
            {
                navigation.Property(pr => pr.Value).HasColumnName(nameof(ProductKind.ProductKindName)).IsRequired();
                navigation.HasIndex(pr => pr.Value);
            });

            builder.HasMany(pk => pk.Products).WithOne(p => p.ProductKind);
        }
    }
}
