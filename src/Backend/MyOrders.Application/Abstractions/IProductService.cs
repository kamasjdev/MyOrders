using MyOrders.Application.DTO;

namespace MyOrders.Application.Abstractions
{
    public interface IProductService
    {
        Task<ProductDto> AddAsync(ProductDto productDto);
        Task<ProductDto> UpdateAsync(ProductDto productDto);
        Task<ProductDto> GetAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task DeleteAsync(int id);
    }
}
