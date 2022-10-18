using MyOrders.Core.Exceptions;

namespace MyOrders.Core.ValueObjects
{
    public class Price : IEquatable<Price>
    {
        public decimal Value { get; }

        public Price(decimal value)
        {
            if (value < 0)
            {
                throw new DomainException($"Price '{value}' is invalid");
            }

            Value = value;
        }

        public bool Equals(Price other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj as Price == null) return false;
            return Equals((Price)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString("0.00");
        }

        public static implicit operator decimal(Price productName)
            => productName.Value;

        public static implicit operator Price(decimal value)
            => new(value);
    }
}
