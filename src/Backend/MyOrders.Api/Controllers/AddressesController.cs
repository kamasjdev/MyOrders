using Microsoft.AspNetCore.Mvc;
using MyOrders.Application.Abstractions;
using MyOrders.Application.DTO;

namespace MyOrders.Api.Controllers
{
    public class AddressesController : BaseController
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AddressDto>> Get(int id)
        {
            return OkOrNotFound(await _addressService.GetAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddressDto addressDto)
        {
            var dto = await _addressService.AddAsync(addressDto);
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:int}")]
        public async Task<AddressDto> Update(int id, AddressDto addressDto)
        {
            return await _addressService.UpdateAsync(addressDto with { Id = id });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _addressService.DeleteAsync(id);
            return NoContent();
        }
    }
}
