using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using test_api.Helpers;
using test_api.Models.Options;
using test_api.Models.Requests;
using test_api.Models.Responses;
using test_api.Services;

namespace test_api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : Controller
    {
        private string JWT_SECRET { get; }
        private readonly WeatherService _weatherService; 

        public WeatherController(IOptions<JWTOptions> options, WeatherService weatherService)
        {
            JWT_SECRET = options.Value.Key;

            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<dynamic> Get() {
            return await _weatherService.search();
        }

        [AllowAnonymous]
        [HttpPost("token/generate", Name = "login")]
        public string Login()
        {
            
            var expires = DateTime.UtcNow.AddDays(1);

            var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            return JwtHelper.CreateToken(JWT_SECRET, ip, expires);
        }
    }
}
