using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace WindowsAuth.Controllers
{
    [ApiController, Route("[controller]"), Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly StaffPermissionsService _staffPermissionsService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, StaffPermissionsService staffPermissionsService)
        {
            _logger = logger;
            _staffPermissionsService = staffPermissionsService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)] + " Dev: " + _staffPermissionsService.IsDeveloper() + "; Comp: " + _staffPermissionsService.IsCompliance()
            })
            .ToArray();
        }
    }
}
