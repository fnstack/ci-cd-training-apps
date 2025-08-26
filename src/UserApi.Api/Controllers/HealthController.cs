using Microsoft.AspNetCore.Mvc;

namespace UserApi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult GetHealth()
    {
        return Ok(new 
        { 
            status = "ok",
            timestamp = DateTime.UtcNow,
            uptime = Environment.TickCount64 / 1000.0 // seconds
        });
    }
}