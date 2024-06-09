using Microsoft.AspNetCore.Mvc;
using TestRedis.Attributes;
using TestRedis.Services;

namespace TestRedis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IResponseCacheService responseCacheService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IResponseCacheService responseCacheService)
        {
            _logger = logger;
            this.responseCacheService = responseCacheService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [Cache(1000)]
        public IActionResult Get(string ok = "nong")
        {
            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToArray();
            return Ok(result);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            responseCacheService.RemoceCacheResponseAsync("/WeatherForecast");
            return Ok();
        }
    }
}
