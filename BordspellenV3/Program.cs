using Domain.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BordspellenDbContext>(opts =>
{
    var connStr = builder.Configuration["ConnectionStrings:BordspelConnection"];

    opts.UseSqlServer(builder.Configuration["ConnectionStrings:BordspelConnection"]);
 
    opts.EnableSensitiveDataLogging(true);
});

builder.Services.AddScoped<IGamesRepository, EFGamesRepository>();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
   // app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllers();
//app.MapControllerRoute("controllers",
 //   "controllers/{controller=Home}/{action=Index}/{id?}");

app.MapDefaultControllerRoute();

app.UseAuthorization();

//app.MapRazorPages();

SeedData.EnsurePopulated(app);

app.Run();

