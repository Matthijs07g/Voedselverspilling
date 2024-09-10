using Microsoft.Extensions.Configuration;
using Voedselverspilling.Application.Services;
using Voedselverspilling.Domain.Interfaces;
using Voedselverspilling.Infrastructure;
using Voedselverspilling.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);



    // Voeg je services toe
//    builder.Services.AddScoped<IStudentService, StudentService>();
//    builder.Services.AddScoped<IKantineService, KantineService>();

    // Voeg je repositories toe
//    builder.Services.AddScoped<IStudentRepository, StudentRepository>();
//    builder.Services.AddScoped<IPakketRepository, PakketRepository>();

    // Voeg je DbContext toe voor Entity Framework
//    builder.Services.AddDbContext<ApplicationDbContext>(options =>
//        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.Run();
