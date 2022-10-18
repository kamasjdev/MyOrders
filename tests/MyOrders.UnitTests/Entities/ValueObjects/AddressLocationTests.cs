using MyOrders.Core.Exceptions;
using MyOrders.Core.ValueObjects;
using Shouldly;

namespace MyOrders.UnitTests.Entities.ValueObjects
{
    public class AddressLocationTests
    {
        [Fact]
        public void should_create_address_location()
        {
            var countryName = "Poland";
            var cityName = "Nowa Sol";
            var streetName = "Szkolna";
            var buildingNumber = 1;

            var addressLocation = AddressLocation.From(countryName, cityName, streetName, buildingNumber);

            addressLocation.ShouldNotBeNull();
            addressLocation.CountryName.ShouldBe(countryName);
            addressLocation.CityName.ShouldBe(cityName);
            addressLocation.StreetName.ShouldBe(streetName);
            addressLocation.BuildingNumber.ShouldBe(buildingNumber);
        }
        
        [Fact]
        public void should_create_address_location_with_flat_number()
        {
            var countryName = "Poland";
            var cityName = "Nowa Sol";
            var streetName = "Szkolna";
            var buildingNumber = 1;
            var flatNumber = 1;

            var addressLocation = AddressLocation.From(countryName, cityName, streetName, buildingNumber, flatNumber);

            addressLocation.ShouldNotBeNull();
            addressLocation.CountryName.ShouldBe(countryName);
            addressLocation.CityName.ShouldBe(cityName);
            addressLocation.StreetName.ShouldBe(streetName);
            addressLocation.BuildingNumber.ShouldBe(buildingNumber);
            addressLocation.FlatNumber.ShouldBe(flatNumber);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void given_invalid_country_name_should_throw_an_exception(string countryName)
        {
            var cityName = "Nowa Sol";
            var streetName = "Szkolna";
            var buildingNumber = 1;
            var flatNumber = 1;

            var exception = Record.Exception(() => AddressLocation.From(countryName, cityName, streetName, buildingNumber, flatNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("Invalid");
        }

        [Theory]
        [InlineData("a")]
        [InlineData("ab")]
        public void given_too_short_country_name_should_throw_an_exception(string countryName)
        {
            var cityName = "Nowa Sol";
            var streetName = "Szkolna";
            var buildingNumber = 1;
            var flatNumber = 1;

            var exception = Record.Exception(() => AddressLocation.From(countryName, cityName, streetName, buildingNumber, flatNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("too short");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void given_invalid_city_name_should_throw_an_exception(string cityName)
        {
            var countryName = "Poland";
            var streetName = "Szkolna";
            var buildingNumber = 1;
            var flatNumber = 1;

            var exception = Record.Exception(() => AddressLocation.From(countryName, cityName, streetName, buildingNumber, flatNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("Invalid");
        }

        [Theory]
        [InlineData("a")]
        [InlineData("ab")]
        public void given_too_short_city_name_should_throw_an_exception(string cityName)
        {
            var countryName = "Poland";
            var streetName = "Szkolna";
            var buildingNumber = 1;
            var flatNumber = 1;

            var exception = Record.Exception(() => AddressLocation.From(countryName, cityName, streetName, buildingNumber, flatNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("too short");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void given_invalid_street_name_should_throw_an_exception(string streetName)
        {
            var countryName = "Poland";
            var cityName = "Nowa Sol";
            var buildingNumber = 1;
            var flatNumber = 1;

            var exception = Record.Exception(() => AddressLocation.From(countryName, cityName, streetName, buildingNumber, flatNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("Invalid");
        }

        [Theory]
        [InlineData("a")]
        [InlineData("ab")]
        public void given_too_short_street_name_should_throw_an_exception(string streetName)
        {
            var countryName = "Poland";
            var cityName = "Nowa Sol";
            var buildingNumber = 1;
            var flatNumber = 1;

            var exception = Record.Exception(() => AddressLocation.From(countryName, cityName, streetName, buildingNumber, flatNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("too short");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void given_invalid_building_number_should_throw_an_exception(int buildingNumber)
        {
            var countryName = "Poland";
            var cityName = "Nowa Sol";
            var streetName = "Szkolna";
            var flatNumber = 1;

            var exception = Record.Exception(() => AddressLocation.From(countryName, cityName, streetName, buildingNumber, flatNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("Invalid");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void given_invalid_flat_number_should_throw_an_exception(int flatNumber)
        {
            var countryName = "Poland";
            var cityName = "Nowa Sol";
            var streetName = "Szkolna";
            var buildingNumber = 1;

            var exception = Record.Exception(() => AddressLocation.From(countryName, cityName, streetName, buildingNumber, flatNumber));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<DomainException>();
            exception.Message.ShouldContain("Invalid");
        }
    }
}
