using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WeStandTogether.Backend.Models.Authentication;

namespace WeStandTogether.Backend.Controllers
{
    [ApiController]
    public class IdentityController: ControllerBase
    {
        private const string TokenSecret = "InRealScenariosThisShouldBeStoredSecurely!";
        private static readonly TimeSpan TokenLifeTime = TimeSpan.FromHours(24);

        [HttpPost("login")]
        public IActionResult GenerateToken([FromBody] LoginRequest request)
        {
            Console.WriteLine("HI");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(TokenSecret);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, request.PhoneNumber),
                new("phone_number", request.PhoneNumber)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenLifeTime),
                Issuer = "WeStandTogether.com",
                Audience = "WeStandTogether.com",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return Ok(jwt);
        }
    }
}
