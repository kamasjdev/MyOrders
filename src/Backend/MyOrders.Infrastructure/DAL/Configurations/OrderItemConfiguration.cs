using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyOrders.Core.Entities;
using MyOrders.Core.ValueObjects;

namespace MyOrders.Infrastructure.DAL.Configurations
{
    internal sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);
            builder.Property(oi => oi.Id)
                .HasConversion(oi => oi.Value, id => new EntityId(id))
                .UseMySqlIdentityColumn();

            builder.Property(oi => oi.Created).IsRequired();
        }
    }
}
