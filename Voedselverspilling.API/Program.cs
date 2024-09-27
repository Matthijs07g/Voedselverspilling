//using Microsoft.AspNetCore.Authentication.JwtBearer;
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

//// 1. Add JWT Authentication
//var jwtKey = builder.Configuration["JwtSettings:SecretKey"];
//var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
//var jwtAudience = builder.Configuration["JwtSettings:Audience"];

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = jwtIssuer,
//        ValidAudience = jwtAudience,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
//    };
//});

// Add CORS configuration (optional)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
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

app.UseAuthentication();
app.UseAuthorization();

// Enable CORS (optional)
app.UseCors("AllowAll");

app.MapControllers();

app.Run();
