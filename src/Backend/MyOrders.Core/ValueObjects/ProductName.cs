using MyOrders.Core.Exceptions;

namespace MyOrders.Core.ValueObjects
{
    public class ProductName : IEquatable<ProductName>
    {
        public string Value { get; }

        public ProductName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException("Invalid ProductName");
            }

            if (value.Length < 3)
            {
                throw new DomainException($"ProductName '{value}' is too short");
            }

            Value = value;
        }

        public bool Equals(ProductName other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj as ProductName == null) return false;
            return Equals((ProductName)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(ProductName productName)
            => productName.Value;

        public static implicit operator ProductName(string value)
            => new(value);
    }
}
