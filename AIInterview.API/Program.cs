using AIInterview.API.Data;
using AIInterview.API.Repostiory.Security;
using AIInterview.API.ServiceConfig;
using AIInterview.API.Services.SecurityService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ?? Services ??????????????????????????????????????????????????
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConnectionConfig(builder.Configuration);
builder.Services.ConfigureServices();

// ?? JWT Configuration ?????????????????????????????????????????
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new Exception("Jwt:Key missing in appsettings.json");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<IJwtService, JwtServiceRepository>();

// ?? Swagger with JWT Authorize button ????????????????????????
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AI Interview API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Neeche Bearer likho phir space phir token\n\nExample: Bearer eyJhbGci..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ?? Build ?????????????????????????????????????????????????????
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();   // ? pehle yeh
app.UseAuthorization();    // ? phir yeh
app.MapControllers();
app.Run();