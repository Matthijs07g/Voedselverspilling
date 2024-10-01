using Microsoft.AspNetCore.Mvc;
using Voedselverspilling.Web.Models;

namespace Voedselverspilling.Web.Controllers
{
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Mvc;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Voedselverspilling.Domain.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "http://localhost:5042/api";
        private readonly UserManager<AppIdentity> _userManager;
        private readonly SignInManager<AppIdentity> _signInManager;


        public AccountController(HttpClient httpClient, UserManager<AppIdentity> userManager, SignInManager<AppIdentity> signInManager)
        {
            _httpClient = httpClient;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginRequest loginRequest)
        {
            // Step 1: Check if the user exists
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

            if (user == null)
            {
                Console.WriteLine("User not found");
                throw new UnauthorizedAccessException("Invalid login attempt"); // User not found
            }

            // Step 2: Check if the password is correct
            var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, true, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("Invalid credentials"); // Incorrect password
            }

            // Step 3: Create the authentication cookie
            await _signInManager.SignInAsync(user, isPersistent: false);

            // Optional: Return user object if needed for client-side purposes (e.g., profile data)
            return RedirectToAction("Mealboxes", "Mealbox"); // Successful login, return the user
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


    }

}
