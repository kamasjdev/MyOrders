using MyOrders.Application.DTO;

namespace MyOrders.Application.Abstractions
{
    public interface IProductKindService
    {
        Task<ProductKindDto> AddAsync(ProductKindDto productKindDto);
        Task<ProductKindDto> UpdateAsync(ProductKindDto productKindDto);
        Task<ProductKindDto> GetAsync(int id);
        Task DeleteAsync(int id);
    }
}
