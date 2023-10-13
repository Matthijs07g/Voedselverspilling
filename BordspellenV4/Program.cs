using Domain.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddDbContext<BordspellenDbContext>(opts =>
{
    var connStr = builder.Configuration["ConnectionStrings:BordspelConnection"];

    opts.UseSqlServer(connStr);

    opts.EnableSensitiveDataLogging(true);
});

builder.Services.AddDbContext<IdentityContext>(opts =>
{
    var connStr = builder.Configuration["ConnectionStrings:IdentityConnection"];

    opts.UseSqlServer(connStr);
});

builder.Services.AddScoped<IGamesRepository, EFGamesRepository>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

SeedData.EnsurePopulated(app);

app.Run();
