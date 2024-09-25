using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Voedselverspilling.Application.Services;
using Voedselverspilling.Domain.Interfaces;
using Voedselverspilling.Domain.IRepositories;
using Voedselverspilling.DomainServices.Services;
using Voedselverspilling.Infrastructure;
using Voedselverspilling.Infrastructure.Repositories;
using Voedselverspilling.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);



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

// Voeg je DbContext toe voor Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("VoedselverspillingDbLocal")));


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
