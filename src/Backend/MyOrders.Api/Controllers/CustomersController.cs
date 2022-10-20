using Microsoft.AspNetCore.Mvc;
using MyOrders.Application.Abstractions;
using MyOrders.Application.DTO;

namespace MyOrders.Api.Controllers
{
    public class CustomersController : BaseController
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IEnumerable<CustomerDto>> GetAll()
        {
            return await _customerService.GetAllAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomerDetailsDto>> Get(int id)
        {
            return OkOrNotFound(await _customerService.GetAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddCustomerDto addCustomerDto)
        {
            var dto = await _customerService.AddAsync(addCustomerDto);
            return base.CreatedAtAction(nameof(Get), new { id = dto.Id }, (object)dto);
        }

        [HttpPut("{id:int}")]
        public async Task<CustomerDto> Update(int id, UpdateCustomerDto dto)
        {
            return await _customerService.UpdateAsync(dto with { Id = id });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _customerService.DeleteAsync(id);
            return NoContent();
        }
    }
}
