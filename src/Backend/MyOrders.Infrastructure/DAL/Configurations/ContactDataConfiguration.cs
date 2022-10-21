using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyOrders.Core.Entities;
using MyOrders.Core.ValueObjects;

namespace MyOrders.Infrastructure.DAL.Configurations
{
    internal sealed class ContactDataConfiguration : IEntityTypeConfiguration<ContactData>
    {
        public void Configure(EntityTypeBuilder<ContactData> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .HasConversion(c => c.Value, id => new EntityId(id));

            builder.OwnsOne(c => c.Email, navigation =>
            {
                navigation.Property(e => e.Value).HasColumnName(nameof(ContactData.Email)).IsRequired().HasMaxLength(300);
                navigation.HasIndex(e => e.Value);
            });

            builder.Property(c => c.PhoneNumber).HasColumnName(nameof(ContactData.PhoneNumber))
                .HasConversion(phone => phone.CountryCode + " " + phone.Numbers, phone => ExtractFromString(phone));

            builder.HasIndex(c => c.PhoneNumber);
        }

        private static PhoneNumber ExtractFromString(string text)
        {
            var splitedString = text.Split(" ");
            var countryCode = splitedString[0];
            var numbers = splitedString[1];
            return PhoneNumber.From(countryCode, numbers);
        }
    }
}
