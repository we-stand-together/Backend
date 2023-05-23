using Microsoft.AspNetCore.Authentication.JwtBearer;







var app = builder.Build();


app.MapGet("/", () => "Hello world");

app.Run();
