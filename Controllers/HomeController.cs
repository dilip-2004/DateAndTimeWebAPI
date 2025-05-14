using DateNadTimeWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DateNadTimeWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllTimeZones()
        {
            var timeZones = TimeZoneInfo.GetSystemTimeZones()
                                        .Where(tz => !tz.Id.Contains("India"))
                                        .Select(tz => new TimeZoneModel
                                        {
                                            DisplayName = tz.DisplayName,
                                            Id = tz.Id,
                                        }).ToList();

            return Ok(timeZones);
        }

        [HttpPost("time")]
        public IActionResult GetTime([FromBody] TimeZoneRequest request)
        {
            if (string.IsNullOrEmpty(request.TimeZoneId))
            {
                return BadRequest("Time zone ID is required.");
            }

            try
            {
                var selectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById(request.TimeZoneId);
                var selectedTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, selectedTimeZone);
                return Ok(new { time = selectedTime.ToString("F") });
            }
            catch (TimeZoneNotFoundException)
            {
                return BadRequest("Invalid time zone ID.");
            }
        }
    }
}