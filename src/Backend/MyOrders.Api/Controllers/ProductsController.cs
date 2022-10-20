using Microsoft.AspNetCore.Mvc;
using MyOrders.Application.Abstractions;
using MyOrders.Application.DTO;

namespace MyOrders.Api.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            return await _productService.GetAllAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> Get(int id)
        {
            return OkOrNotFound(await _productService.GetAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> Add(ProductDto productDto)
        {
            var dto = await _productService.AddAsync(productDto);
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:int}")]
        public async Task<ProductDto> Update(int id, ProductDto productDto)
        {
            return await _productService.UpdateAsync(productDto with { Id = id });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}
