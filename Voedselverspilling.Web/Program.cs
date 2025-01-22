using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Voedselverspilling.DomainServices.Interfaces;
using Voedselverspilling.DomainServices.IRepositories;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.Infrastructure;
using Voedselverspilling.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add your DbContext for Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VoedselverspillingDbLocal")));

builder.Services.AddDbContext<IdDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDbLocal")));

// Register Identity services
builder.Services.AddIdentity<AppIdentity, IdentityRole>(options =>
{
    // Configure Identity options as necessary
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<IdDbContext>()
.AddDefaultTokenProviders();

// Configure cookie-based authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(3); // Set cookie expiration
        options.SlidingExpiration = true; // Allows the cookie to be refreshed on activity
        options.LoginPath = "/Account/Login"; // Redirect here on login failure
        options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect here on access denied
    });

// Add repositories
builder.Services.AddScoped<IKantineRepository, KantineRepository>();
builder.Services.AddScoped<IKantineWorkerRepository, KantineWorkerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IPakketRepository, PakketRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Apply pending migrations automatically
    var context = services.GetRequiredService<IdDbContext>();
    context.Database.Migrate();

    // Seed roles and users
    var userManager = services.GetRequiredService<UserManager<AppIdentity>>();
    var signInManager = services.GetRequiredService<SignInManager<AppIdentity>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await SeedRolesAndUsersAsync(userManager, roleManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Ensure this is before UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

async Task SeedRolesAndUsersAsync(UserManager<AppIdentity> userManager, RoleManager<IdentityRole> roleManager)
{
    // Seed roles if not already present
    string[] roleNames = { "Admin", "Worker", "Student" };
    foreach (var roleName in roleNames)
    {
        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (roleResult.Succeeded)
            {
                Console.WriteLine($"Role {roleName} created successfully.");
            }
            else
            {
                Console.WriteLine($"Error creating role {roleName}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }
        }
    }

    // Seed Worker user
    var workerEmail = "i.jansen@avans.nl";
    var workerUser = await userManager.FindByEmailAsync(workerEmail);
    if (workerUser == null)
    {
        var worker = new AppIdentity
        {
            UserName = workerEmail,
            Email = workerEmail,
            Rol = "Worker", // Ensure your AppIdentity class has a property for roles if needed
            EmailConfirmed = true
        };

        var password = "123"; // Secure password
        var createWorker = await userManager.CreateAsync(worker, password);

        if (createWorker.Succeeded)
        {
            await userManager.AddToRoleAsync(worker, "Worker");
            Console.WriteLine($"Worker user {workerEmail} created successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to create Worker user: {string.Join(", ", createWorker.Errors.Select(e => e.Description))}");
        }
    }

    var workerEmail1 = "h.basten@avans.nl";
    var workerUser1 = await userManager.FindByEmailAsync(workerEmail1);
    if (workerUser1 == null)
    {
        var worker = new AppIdentity
        {
            UserName = workerEmail1,
            Email = workerEmail1,
            Rol = "Worker", // Ensure your AppIdentity class has a property for roles if needed
            EmailConfirmed = true
        };

        var password = "123"; // Secure password
        var createWorker = await userManager.CreateAsync(worker, password);

        if (createWorker.Succeeded)
        {
            await userManager.AddToRoleAsync(worker, "Worker");
            Console.WriteLine($"Worker user {workerEmail} created successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to create Worker user: {string.Join(", ", createWorker.Errors.Select(e => e.Description))}");
        }
    }

    // Seed Student user
    var studentEmail = "mmj.vangastel@student.avans.nl";
    var studentUser = await userManager.FindByEmailAsync(studentEmail);
    if (studentUser == null)
    {
        var student = new AppIdentity
        {
            UserName = studentEmail,
            Email = studentEmail,
            Rol = "Student",
            EmailConfirmed = true
        };

        var password = "matthijs"; // Secure password
        var createStudent = await userManager.CreateAsync(student, password);

        if (createStudent.Succeeded)
        {
            await userManager.AddToRoleAsync(student, "Student");
            Console.WriteLine($"Student user {studentEmail} created successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to create Student user: {string.Join(", ", createStudent.Errors.Select(e => e.Description))}");
        }
    }
    var student1Email = "t.jansen@student.avans.nl";
    var student1User = await userManager.FindByEmailAsync(student1Email);
    if (student1User == null)
    {
        var student = new AppIdentity
        {
            UserName = student1Email,
            Email = student1Email,
            Rol = "Student",
            EmailConfirmed = true
        };

        var password = "123"; // Secure password
        var createStudent = await userManager.CreateAsync(student, password);

        if (createStudent.Succeeded)
        {
            await userManager.AddToRoleAsync(student, "Student");
            Console.WriteLine($"Student user {studentEmail} created successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to create Student user: {string.Join(", ", createStudent.Errors.Select(e => e.Description))}");
        }
    }

    // Seed admin user
    var adminEmail = "admin@mail.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var admin = new AppIdentity
        {
            UserName = adminEmail,
            Email = adminEmail,
            Rol = "Admin",
            EmailConfirmed = true,
        };

        var password = "admin"; // Secure password
        var createAdmin = await userManager.CreateAsync(admin, password);

        if (createAdmin.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
            Console.WriteLine($"Admin user {adminEmail} created successfully.");
        }
        else
        {
            Console.WriteLine($"Failed to create Admin user: {string.Join(", ", createAdmin.Errors.Select(e => e.Description))}");
        }
    }
}
