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
    using System.Net.Http.Headers;

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
                return Unauthorized("Invalid login attempt"); // User not found
            }

            // Step 2: Check if the password is correct
            var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, true, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Unauthorized("Invalid credentials"); // Incorrect password
            }

            // Step 3: Call the /Account/Login endpoint to generate the JWT
            var httpClient = new HttpClient();
            var loginRequestPayload = new
            {
                Email = loginRequest.Email,
                Password = loginRequest.Password
            };

            // Assume the /Account/Login endpoint returns a JSON object with a "token" property
            var response = await httpClient.PostAsJsonAsync($"{_apiBaseUrl}/Account/login", loginRequestPayload);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error logging in.");
            }

            var jwtResponse = await response.Content.ReadFromJsonAsync<JwtResponse>();
            if (jwtResponse == null || string.IsNullOrEmpty(jwtResponse.Token))
            {
                return StatusCode((int)response.StatusCode, "Failed to retrieve token.");
            }

            // Step 4: Store the JWT in an HttpOnly cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,    // Ensure that the cookie can't be accessed via JavaScript
                Secure = true,      // Ensure the cookie is only sent over HTTPS
                SameSite = SameSiteMode.Strict, // Prevent CSRF attacks
                Expires = DateTime.UtcNow.AddMinutes(30) // Adjust expiration as needed
            };

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtResponse.Token);

            // Step 5: Add the JWT to a secure cookie
            Response.Cookies.Append("jwt", jwtResponse.Token, cookieOptions);

            // Optional: Return user object if needed for client-side purposes (e.g., profile data)
            return RedirectToAction("Index", "Home"); // Successful login, return the user
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
