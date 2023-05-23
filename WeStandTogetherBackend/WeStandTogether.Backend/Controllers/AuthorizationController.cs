using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WeStandTogether.Backend.Models.Authentication;
using WeStandTogether.Dapper;

namespace WeStandTogether.Backend.Controllers;

[ApiController]
public class AuthorizationController : ControllerBase
{
    public AuthorizationController(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    private readonly DapperContext _dapperContext;

    private const string TokenSecret = "InRealScenariosThisShouldBeStoredSecurely!";
    private static readonly TimeSpan TokenLifeTime = TimeSpan.FromHours(24);

    [HttpPost("login")]
    public IActionResult GenerateToken([FromBody] AuthorizationRequest request)
    {
        Console.WriteLine("Login");

        using var dbConnection = _dapperContext.CreateConnection();
        dbConnection.Open();

        var command = dbConnection.CreateCommand();
        command.CommandText =
            $"SELECT count(*) FROM Users WHERE phone_number = '{request.PhoneNumber}' AND password = '{request.Password}';";

        var usersCount = (Int64)(command.ExecuteScalar() ?? throw new Exception("Database Error"));

        if (usersCount is 0)
        {
            return BadRequest("Wrong Phone Number Or Password");
        }
        
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

    [HttpPost("register")]
    public IActionResult Register([FromBody] AuthorizationRequest request)
    {
        Console.WriteLine("Register");
        using var dbConnection = _dapperContext.CreateConnection();
        dbConnection.Open();

        var command = dbConnection.CreateCommand();
        command.CommandText =
            $"insert into Users (phone_number, password) values ('{request.PhoneNumber}', '{request.Password}');";

        command.ExecuteNonQuery();
        return Ok();
    }
}