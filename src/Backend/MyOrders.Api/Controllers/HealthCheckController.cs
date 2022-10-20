using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyOrders.Infrastructure.App;

namespace MyOrders.Api.Controllers
{
    [Route("api")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly AppOptions _appOptions;

        public HealthCheckController(IOptionsMonitor<AppOptions> appOptions)
        {
            _appOptions = appOptions.CurrentValue;
        }

        [HttpGet]
        public ActionResult<string> Get() => Ok(_appOptions.Name);
    }
}
