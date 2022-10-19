using MyOrders.Core.ValueObjects;

namespace MyOrders.Core.Entities
{
    public interface IBaseEntity
    {
        EntityId Id { get; }
    }
}
