using MyOrders.Core.Exceptions;

namespace MyOrders.Core.ValueObjects
{
    public class OrderNumber
    {
        public string Value { get; }

        public OrderNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainException("Invalid OrderNumber");
            }

            if (value.Length < 3)
            {
                throw new DomainException($"OrderNumber '{value}' is too short");
            }

            Value = value;
        }

        public bool Equals(OrderNumber other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj as OrderNumber == null) return false;
            return Equals((OrderNumber)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(OrderNumber orderNumber)
            => orderNumber.Value;

        public static implicit operator OrderNumber(string value)
            => new(value);
    }
}
