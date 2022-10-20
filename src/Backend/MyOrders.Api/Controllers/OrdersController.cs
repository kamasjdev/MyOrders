using Microsoft.AspNetCore.Mvc;
using MyOrders.Application.Abstractions;
using MyOrders.Application.DTO;

namespace MyOrders.Api.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDetailsDto>> Get(int id)
        {
            return OkOrNotFound(await _orderService.GetAsync(id));
        }

        [HttpGet("by-customer/{customerId:int}")]
        public async Task<IEnumerable<OrderDto>> GetByCustomer(int customerId)
        {
            return await _orderService.GetByCustomerIdAsync(customerId);
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddOrderDto addOrderDto)
        {
            var dto = await _orderService.AddAsync(addOrderDto);
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:int}")]
        public async Task<OrderDetailsDto> Update(int id, UpdateOrderDto dto)
        {
            return await _orderService.UpdateAsync(dto with { Id = id });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _orderService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("price/{id:int}")]
        public async Task<OrderDto> UpdatePrice(int id, UpdateOrderPriceDto updateOrderPriceDto)
        {
            return await _orderService.UpdatePriceAsync(updateOrderPriceDto with { Id = id });
        }
    }
}
