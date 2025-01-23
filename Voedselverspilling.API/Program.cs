// using Microsoft.AspNetCore.Authentication.JwtBearer; // Uncomment this line
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Voedselverspilling.Infrastructure.Repositories;
using Voedselverspilling.Infrastructure;
using Voedselverspilling.DomainServices.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Voedselverspilling.Domain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Voedselverspilling.DomainServices.IRepositories;
using Voedselverspilling.DomainServices.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register repositories
builder.Services.AddScoped<IKantineRepository, KantineRepository>();
builder.Services.AddScoped<IKantineWorkerRepository, KantineWorkerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IPakketRepository, PakketRepository>();



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
