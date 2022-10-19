using MyOrders.Core.ValueObjects;

namespace MyOrders.Core.Entities
{
    public class ProductKind : IBaseEntity
    {
        public EntityId Id { get; }
        public ProductKindName ProductKindName { get; private set; }

        public ProductKind(EntityId id, ProductKindName productKindName)
        {
            Id = id;
            ProductKindName = productKindName;
        }

        public static ProductKind Create(ProductKindName productKindName)
        {
            return new ProductKind(0, productKindName);
        }

        public void ChangeProductKindName(ProductKindName productKindName)
        {
            ProductKindName = productKindName;
        }
    }
}
