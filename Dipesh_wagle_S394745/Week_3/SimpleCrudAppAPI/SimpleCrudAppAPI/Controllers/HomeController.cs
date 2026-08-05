using Microsoft.AspNetCore.Mvc;

namespace SimpleCrudAppAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                message = "Welcome to SimpleCrudAppAPI",
                version = "1.0",
                endpoints = new
                {
                    weatherForecast = "/weatherforecast"
                }
            });
        }
    }
}
