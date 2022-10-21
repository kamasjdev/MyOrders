using MyOrders.Core.Exceptions;

namespace MyOrders.Core.ValueObjects
{
    public class ZipCode : IEquatable<ZipCode>
    {
        public string Value { get; }

        public ZipCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException("Invalid ZipCode");
            }

            Value = value;
        }

        public bool Equals(ZipCode other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj as ZipCode == null) return false;
            return Equals((ZipCode)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(ZipCode zipCode)
            => zipCode.Value;

        public static implicit operator ZipCode(string zipCode)
            => new(zipCode);
    }
}
