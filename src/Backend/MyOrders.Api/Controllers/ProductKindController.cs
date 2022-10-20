using Microsoft.AspNetCore.Mvc;
using MyOrders.Application.Abstractions;
using MyOrders.Application.DTO;

namespace MyOrders.Api.Controllers
{
    public class ProductKindsController : BaseController
    {
        private readonly IProductKindService _productKindService;

        public ProductKindsController(IProductKindService productKindService)
        {
            _productKindService = productKindService;
        }

        [HttpGet]
        public async Task<IEnumerable<ProductKindDto>> GetAll()
        {
            return await _productKindService.GetAllAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductKindDto>> Get(int id)
        {
            var productKind = await _productKindService.GetAsync(id);
            return OkOrNotFound(productKind);
        }

        [HttpPost]
        public async Task<ActionResult> Add(ProductKindDto productKindDto)
        {
            var dto = await _productKindService.AddAsync(productKindDto);
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:int}")]
        public async Task<ProductKindDto> Update(int id, ProductKindDto productKindDto)
        {
            return await _productKindService.UpdateAsync(productKindDto with { Id = id });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _productKindService.DeleteAsync(id);
            return NoContent();
        }
    }
}
