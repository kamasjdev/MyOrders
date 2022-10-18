using MyOrders.Core.Exceptions;

namespace MyOrders.Core.ValueObjects
{
    public class Person : IEquatable<Person>
    {
        public string FirstName { get; }
        public string LastName { get; }

        public Person(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new DomainException("Invalid FirstName");
            }

            if (firstName.Length < 3)
            {
                throw new DomainException($"FirstName '{firstName}' is too short");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new DomainException("Invalid LastName");
            }

            if (lastName.Length < 3)
            {
                throw new DomainException($"LastName '{lastName}' is too short");
            }

            FirstName = firstName;
            LastName = lastName;
        }

        public static Person From(string firstName, string lastName)
        {
            return new Person(firstName, lastName);
        }

        public bool Equals(Person other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj as Person == null) return false;
            return Equals((Person)obj);
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        private IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
        }
    }
}
