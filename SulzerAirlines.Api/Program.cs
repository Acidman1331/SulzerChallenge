using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SulzerAirlines.Api.Middleware;
using SulzerAirlines.Application.PriceCalculation;
using SulzerAirlines.Application.Services;
using SulzerAirlines.Application.Services.Interfaces;
using SulzerAirlines.Domain.Interfaces;
using SulzerAirlines.Infrastructure.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddSingleton<IFlightRepository, FlightRepository>();
builder.Services.Configure<TimeFactorRulesOptions>(builder.Configuration.GetSection("TimeFactorRules"));
builder.Services.AddScoped<ITimeFactorProviderFactory, TimeFactorProviderFactory>();
builder.Services.AddScoped<IPriceCalculator, PriceCalculator>();
builder.Services.AddScoped<IFindRoutesService, FindRoutesService>();
builder.Services.AddScoped<IBookingService, BookingService>();

var jwtKey = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "");
var issuer = builder.Configuration["Jwt:Issuer"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtKey)
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Martin API", Version = "v1" });

    // Configuración para usar JWT Bearer en Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Bearer K3Y.5UP3r!?¡!Secreta_1234SG789o!@#$"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthorization();


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapPost("/login", ([FromBody] UserLogin login) =>
{
    if (login.Username == builder.Configuration["Login:User"] &&
        login.Password == builder.Configuration["Login:Passw"])
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, login.Username),
             new Claim(ClaimTypes.Role, "Admin")
         };

        var key = new SymmetricSecurityKey(jwtKey);

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }

    return Results.Unauthorized();
});


app.Run();

public record UserLogin
{
    public string Username { get; init; } = "test";
    public string Password { get; init; } = "123";
}