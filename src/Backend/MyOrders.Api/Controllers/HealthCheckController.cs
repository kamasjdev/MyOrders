using Microsoft.AspNetCore.Mvc;

namespace MyOrders.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "MyOrders Api";
        }
    }
}
