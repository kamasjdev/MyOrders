using Microsoft.AspNetCore.Mvc;
using MyOrders.Application.Abstractions;
using MyOrders.Application.DTO;

namespace MyOrders.Api.Controllers
{
    public class ContactDatasController : BaseController
    {
        private readonly IContactDataService _contactDataService;

        public ContactDatasController(IContactDataService contactDataService)
        {
            _contactDataService = contactDataService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ContactDataDto>> Get(int id)
        {
            return OkOrNotFound(await _contactDataService.GetAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult> Add(ContactDataDto contactDataDto)
        {
            var dto = await _contactDataService.AddAsync(contactDataDto);
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpPut("{id:int}")]
        public async Task<ContactDataDto> Update(int id, ContactDataDto contactDataDto)
        {
            return await _contactDataService.UpdateAsync(contactDataDto with { Id = id });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _contactDataService.DeleteAsync(id);
            return NoContent();
        }
    }
}
