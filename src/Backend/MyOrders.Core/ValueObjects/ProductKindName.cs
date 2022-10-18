using MyOrders.Core.Exceptions;

namespace MyOrders.Core.ValueObjects
{
    public class ProductKindName : IEquatable<ProductKindName>
    {
        public string Value { get; }

        public ProductKindName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException("Invalid ProductKindName");
            }

            if (value.Length < 3)
            {
                throw new DomainException($"ProductKindName '{value}' is too short");
            }

            Value = value;
        }

        public bool Equals(ProductKindName other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj as ProductKindName == null) return false;
            return Equals((ProductKindName)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(ProductKindName productKindName)
            => productKindName.Value;

        public static implicit operator ProductKindName(string value)
            => new(value);
    }
}
