using System;
using System.Web.Http;

namespace NewLife.Controllers
{
    public class HealthController : ApiController
    {
        [HttpGet]
        [Route("api/health")]
        public IHttpActionResult Get()
        {
            return Ok(new { status = "ok", ts = DateTime.UtcNow });
        }
    }
}
