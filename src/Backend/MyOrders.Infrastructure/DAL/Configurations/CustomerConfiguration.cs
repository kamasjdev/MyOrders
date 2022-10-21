using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyOrders.Core.Entities;
using MyOrders.Core.ValueObjects;

namespace MyOrders.Infrastructure.DAL.Configurations
{
    internal sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasConversion(c => c.Value, id => new EntityId(id))
                .UseMySqlIdentityColumn();

            builder.OwnsOne(c => c.Person, navigation =>
            {
                navigation.Property(p => p.FirstName).HasColumnName(nameof(Customer.Person.FirstName)).HasMaxLength(200).IsRequired();
                navigation.Property(p => p.LastName).HasColumnName(nameof(Customer.Person.LastName)).HasMaxLength(200).IsRequired();

                navigation.HasIndex(i => new { i.FirstName, i.LastName });
            });

            builder.HasOne(c => c.Address).WithOne(a => a.Customer).HasForeignKey<Customer>(c => c.AddressId);
            builder.HasOne(c => c.ContactData).WithOne(a => a.Customer).HasForeignKey<Customer>(c => c.ContactDataId);

            builder.HasMany(c => c.OrderItems).WithOne(oi => oi.Customer);
            builder.HasMany(c => c.Orders).WithOne(o => o.Customer);
        }
    }
}
