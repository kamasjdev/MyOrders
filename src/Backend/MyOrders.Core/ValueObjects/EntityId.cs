using MyOrders.Core.Exceptions;

namespace MyOrders.Core.ValueObjects
{
    public class EntityId : IEquatable<EntityId>
    {
        public int Value { get; }

        public EntityId(int id)
        {
            if (id < 0)
            {
                throw new DomainException("Invalid EntityId");
            }

            Value = id;
        }

        public bool Equals(EntityId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj as EntityId == null) return false;
            return Equals((EntityId)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"id: {Value}";
        }

        public static implicit operator int(EntityId entityId)
            => entityId.Value;

        public static implicit operator EntityId(int value)
            => new(value);
    }
}
