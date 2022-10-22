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
                .UseMySqlIdentityColumn();

            builder.Property(c => c.Email)
                .HasConversion(c => c.Value, mail => new Email(mail))
                .IsRequired()
                .HasMaxLength(300);
            builder.HasIndex(c => c.Email);

            builder.Property(c => c.PhoneNumber).HasColumnName(nameof(ContactData.PhoneNumber))
                .HasConversion(phone => phone.CountryCode + " " + phone.Numbers, phone => ExtractFromString(phone))
                .IsRequired().HasMaxLength(100);

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
