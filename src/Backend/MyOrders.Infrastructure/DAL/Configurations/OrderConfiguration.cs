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
                .UseMySqlIdentityColumn();

            builder.Property(o => o.OrderNumber)
                .HasConversion(o => o.Value, number => new OrderNumber(number))
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(p => p.OrderNumber).IsUnique();


            builder.Property(o => o.Price)
                .HasConversion(o => o.Value, price => new Price(price))
                .IsRequired()
                .HasPrecision(14, 4);

            builder.Property(o => o.Created).IsRequired();

            builder.HasMany(o => o.OrderItems).WithOne(oi => oi.Order);
        }
    }
}
