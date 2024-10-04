// using Microsoft.AspNetCore.Authentication.JwtBearer; // Uncomment this line
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Voedselverspilling.Application.Services;
using Voedselverspilling.Domain.Interfaces;
using Voedselverspilling.Infrastructure.Repositories;
using Voedselverspilling.Infrastructure;
using Voedselverspilling.Infrastructure.Services;
using Voedselverspilling.Domain.IRepositories;
using Voedselverspilling.DomainServices.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.DomainServices.IServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register services and repositories
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IKantineService, KantineService>();
builder.Services.AddScoped<IKantineWorkerService, KantineWorkerService>();
builder.Services.AddScoped<IPakketService, PakketService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IReserveringService, ReserveringService>();
builder.Services.AddScoped<IAccountService, AccountService>();

// Register repositories
builder.Services.AddScoped<IKantineRepository, KantineRepository>();
builder.Services.AddScoped<IKantineWorkerRepository, KantineWorkerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IReserveringRepository, ReserveringRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IPakketRepository, PakketRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();

// Add your DbContext for Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VoedselverspillingDbLocal")));

// Identity DbContext
builder.Services.AddDbContext<IdDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDbLocal")));

builder.Services.AddIdentity<AppIdentity, IdentityRole>()
    .AddEntityFrameworkStores<IdDbContext>()
    .AddDefaultTokenProviders();

// 1. Add JWT Authentication Configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true, // Validate token expiration
        ClockSkew = TimeSpan.Zero  // Optional: reduce default clock skew of 5 mins
    };
});

// Add CORS configuration (optional)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS (optional)
app.UseCors("AllowAll");

// Enable authentication and authorization middleware
app.UseAuthentication(); // Add this line to enable JWT token validation
app.UseAuthorization();

app.MapControllers();

app.Run();
