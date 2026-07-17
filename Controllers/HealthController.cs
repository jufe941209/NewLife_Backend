using Microsoft.AspNetCore.Mvc;

namespace NewLife.Controllers
{
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [Route("api/health")]
        public IActionResult Get()
        {
            return Ok(new { status = "ok", ts = DateTime.UtcNow });
        }
    }
}
