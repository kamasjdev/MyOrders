using MyOrders.Core.Exceptions;

namespace MyOrders.Core.ValueObjects
{
    public class PhoneNumber : IEquatable<PhoneNumber>
    {
        public string CountryCode { get; }
        public string Numbers { get; }

        public PhoneNumber(string countryCode, string numbers)
        {
            if (string.IsNullOrWhiteSpace(countryCode))
            {
                throw new DomainException($"Invalid CountryCode in PhoneNumber");
            }

            if (string.IsNullOrWhiteSpace(numbers))
            {
                throw new DomainException($"Invalid Numbers in PhoneNumber");
            }

            if (!countryCode.StartsWith('+'))
            {
                throw new DomainException($"Invalid format CountryCode in PhoneNumber");
            }

            var countryCodeToCheck = countryCode.Replace("+", string.Empty);
            CountryCode = countryCodeToCheck.All(char.IsDigit)
                ? countryCode
                : throw new DomainException($"Invalid CountryCode '{countryCode}' in PhoneNumber. Allowed digits starts with plus ('+') for example '+48'");
            Numbers = numbers.All(char.IsDigit)
                ? numbers
                : throw new DomainException($"Invalid Numbers '{numbers}' in PhoneNumber. Allowed digits only.");
        }

        public static PhoneNumber From(string countryCode, string numbers)
        {
            return new PhoneNumber(countryCode, numbers);
        }

        public bool Equals(PhoneNumber other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj as PhoneNumber == null) return false;
            return Equals((PhoneNumber)obj);
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        public override string ToString()
        {
            return $"+{CountryCode} {Numbers}";
        }

        private IEnumerable<object> GetEqualityComponents()
        {
            yield return CountryCode;
            yield return Numbers;
        }

    }
}
