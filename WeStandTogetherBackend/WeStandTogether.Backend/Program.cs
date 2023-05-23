using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
 


var app = builder.Build();


app.MapGet("/", () => "Hello world");

app.Run();
