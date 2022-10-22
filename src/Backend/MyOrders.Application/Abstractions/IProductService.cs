using MyOrders.Application.DTO;

namespace MyOrders.Application.Abstractions
{
    public interface IProductService
    {
        Task<ProductDetailsDto> AddAsync(ProductDetailsDto productDto);
        Task<ProductDetailsDto> UpdateAsync(ProductDetailsDto productDto);
        Task<ProductDetailsDto> GetAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task DeleteAsync(int id);
    }
}
