using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyOrders.Core.Entities;
using MyOrders.Core.ValueObjects;

namespace MyOrders.Infrastructure.DAL.Configurations
{
    internal sealed class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                .UseMySqlIdentityColumn();

            builder.OwnsOne(a => a.AddressLocation, navigation =>
            {
                navigation.Property(ad => ad.CountryName).HasColumnName(nameof(Address.AddressLocation.CountryName)).IsRequired().HasMaxLength(200);
                navigation.Property(ad => ad.CityName).HasColumnName(nameof(Address.AddressLocation.CityName)).IsRequired().HasMaxLength(200);
                navigation.Property(ad => ad.StreetName).HasColumnName(nameof(Address.AddressLocation.StreetName)).IsRequired().HasMaxLength(200);
                navigation.Property(ad => ad.BuildingNumber).HasColumnName(nameof(Address.AddressLocation.BuildingNumber)).IsRequired();
                navigation.Property(ad => ad.FlatNumber).HasColumnName(nameof(Address.AddressLocation.FlatNumber));
            });

            builder.Property(a => a.ZipCode)
                .HasConversion(a => a.Value, code => new ZipCode(code))
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(p => p.ZipCode);
        }
    }
}
