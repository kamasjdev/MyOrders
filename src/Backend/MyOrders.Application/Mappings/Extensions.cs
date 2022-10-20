using MyOrders.Application.DTO;
using MyOrders.Core.Entities;

namespace MyOrders.Application.Mappings
{
    internal static class Extensions
    {
        public static ProductDto AsDto(this Product product)
        {
            return new ProductDto(product.Id, product.ProductName.Value, product.ProductKind.AsDto(), product.Price);
        }

        public static ProductKindDto AsDto(this ProductKind productKind)
        {
            return new ProductKindDto(productKind.Id, productKind.ProductKindName.Value);
        }
    }
}
