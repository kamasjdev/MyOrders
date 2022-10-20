using Microsoft.AspNetCore.Mvc;
using MyOrders.Application.Abstractions;
using MyOrders.Application.DTO;

namespace MyOrders.Api.Controllers
{
    public class OrderItemsController : BaseController
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemsController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet("not-ordered")]
        public async Task<IEnumerable<OrderItemDto>> Get([FromQuery] int customerId)
        {
            return await _orderItemService.GetAllNotOrderedByCustomerIdAsync(customerId);
        }

        [HttpPost]
        public async Task<OrderItemDto> Post(AddOrderItemDto addOrderItemDto)
        {
            return await _orderItemService.AddAsync(addOrderItemDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _orderItemService.DeleteAsync(id);
            return NoContent();
        }
    }
}
