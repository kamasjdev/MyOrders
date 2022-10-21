using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyOrders.Core.Entities;
using MyOrders.Core.ValueObjects;

namespace MyOrders.Infrastructure.DAL.Configurations
{
    internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id)
                .HasConversion(o => o.Value, id => new EntityId(id));

            builder.OwnsOne(o => o.OrderNumber, navigation =>
            {
                navigation.Property(number => number.Value).HasColumnName(nameof(Order.OrderNumber)).IsRequired();
                navigation.HasIndex(number => number.Value).IsUnique();
            });

            builder.OwnsOne(o => o.Price, navigation =>
            {
                navigation.Property(number => number.Value).HasColumnName(nameof(Order.Price)).IsRequired();
            });

            builder.Property(o => o.Created).IsRequired();

            builder.HasMany(o => o.OrderItems).WithOne(oi => oi.Order);
        }
    }
}
