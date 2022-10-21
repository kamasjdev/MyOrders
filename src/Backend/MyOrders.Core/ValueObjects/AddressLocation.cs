using MyOrders.Core.Exceptions;

namespace MyOrders.Core.ValueObjects
{
    public class AddressLocation : IEquatable<AddressLocation>
    {
        public string StreetName { get; }
        public int BuildingNumber { get; }
        public int? FlatNumber { get; } = null;
        public string CityName { get; }
        public string CountryName { get; }

        private AddressLocation()
        { }

        public AddressLocation(string countryName, string cityName, string streetName, int buildingNumber, int? flatNumber = null)
        {
            ValidateCountryName(countryName);
            ValidateCityName(cityName);
            ValidateStreetName(streetName);
            ValidateBuildingNumber(buildingNumber);

            CountryName = countryName;
            CityName = cityName;
            StreetName = streetName;
            BuildingNumber = buildingNumber;

            if (flatNumber.HasValue)
            {
                ValidateFlatNumber(flatNumber.Value);
                FlatNumber = flatNumber;
            }
        }

        public static AddressLocation From(string countryName, string cityName, string streetName, int buildingNumber)
        {
            return new AddressLocation(countryName, cityName, streetName, buildingNumber);
        }

        public static AddressLocation From(string countryName, string cityName, string streetName, int buildingNumber, int flatNumber)
        {
            return new AddressLocation(countryName, cityName, streetName, buildingNumber, flatNumber);
        }

        public bool Equals(AddressLocation other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj as AddressLocation == null) return false;
            return Equals((AddressLocation)obj);
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        private IEnumerable<object> GetEqualityComponents()
        {
            yield return StreetName;
            yield return BuildingNumber;
            yield return FlatNumber;
            yield return CityName;
            yield return CountryName;
        }

        public override string ToString()
        {
            if (FlatNumber == null)
            {
                return $"{CountryName}, {CityName}, {StreetName} {BuildingNumber}";
            }

            return $"{CountryName}, {CityName}, {StreetName} {BuildingNumber}/{FlatNumber}";
        }

        private static void ValidateStreetName(string streetName)
        {
            if (string.IsNullOrWhiteSpace(streetName))
            {
                throw new DomainException("Invalid StreetName");
            }

            if (streetName.Length < 3)
            {
                throw new DomainException($"StreetName: '{streetName}' is too short");
            }
        }

        private static void ValidateBuildingNumber(int buildingNumber)
        {
            if (buildingNumber < 1)
            {
                throw new DomainException("Invalid BuildingNumber");
            }
        }

        private static void ValidateFlatNumber(int flatNumber)
        {
            if (flatNumber < 1)
            {
                throw new DomainException("Invalid FlatNumber");
            }
        }

        private static void ValidateCityName(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new DomainException("Invalid City");
            }

            if (city.Length < 3)
            {
                throw new DomainException($"City: '{city}' is too short");
            }
        }

        private static void ValidateCountryName(string countryName)
        {
            if (string.IsNullOrWhiteSpace(countryName))
            {
                throw new DomainException("Invalid CountryName");
            }

            if (countryName.Length < 3)
            {
                throw new DomainException($"CountryName: '{countryName}' is too short");
            }
        }
    }
}
