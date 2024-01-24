using BusinessObjects.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BeanFastApi.Validators;

namespace BeanFastApi.Controllers
{
    //[ApiController]
    //[CustomAuthorized(RoleName.ADMIN, RoleName.MANAGER)]
    //[Route("[controller]")]
    public class WeatherForecastController : BaseController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }
        [HttpGet("hello")]
        public string Hello()
        {

            return "Hello___v2____hello";
        }

        [HttpGet(Name = "GetWeatherForecast")]
        [RoleBaseAuthorize(RoleName.ADMIN)]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet("token")]
        public string CreateToken()
        {
            return generateToken();
        }
        private string generateToken()
        {
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey secrectKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes("HELLOWORLD_THIS_IS_THE_SECRET_KEY"));
            var credentials = new SigningCredentials(secrectKey, SecurityAlgorithms.HmacSha256Signature);
            List<Claim> claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, "thanh"),
            new Claim(ClaimTypes.Role, RoleName.ADMIN.ToString()),
        };
            //if (guidClaim != null) claims.Add(new Claim(guidClaim.Item1, guidClaim.Item2.ToString()));
            var expires = DateTime.Now.AddDays(1);
            var token = new JwtSecurityToken(expires: expires, claims: claims, signingCredentials: credentials);
            return jwtHandler.WriteToken(token);
        }
    }   
}
