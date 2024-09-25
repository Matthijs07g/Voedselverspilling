using Voedselverspilling.Application.Services;
using Voedselverspilling.Domain.Interfaces;
using Voedselverspilling.Infrastructure.Repositories;
using Voedselverspilling.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Voedselverspilling.Infrastructure.Services;
using Voedselverspilling.Domain.IRepositories;
using Voedselverspilling.DomainServices.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Voeg je services toe
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IKantineService, KantineService>();
builder.Services.AddScoped<IKantineWorkerService, KantineWorkerService>();
builder.Services.AddScoped<IPakketService, PakketService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IReserveringService, ReserveringService>();

// Voeg je repositories toe
builder.Services.AddScoped<IKantineRepository, KantineRepository>();
builder.Services.AddScoped<IKantineWorkerRepository, KantineWorkerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IReserveringRepository, ReserveringRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IPakketRepository, PakketRepository>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>();

// Voeg je DbContext toe voor Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
   { options.UseSqlServer(builder.Configuration.GetConnectionString("VoedselverspillingDbLocal")); });

// Identity DbContext
builder.Services.AddDbContext<IdDbContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDbLocal")); });

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

app.UseAuthorization();
app.UseAuthentication();

// Enable CORS (optional)
app.UseCors("AllowAll");

app.MapControllers();

app.Run();
