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

    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "http://localhost:5042/api";


        public AccountController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginUser(LoginRequest loginRequest)
        {
            // Validate the incoming login request
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(loginRequest); // Return to login view with error messages
            }

            // Convert the login request to JSON
            var jsonContent = JsonSerializer.Serialize(loginRequest);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Send the request to the API
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/Account/login", contentString);

            if (response.IsSuccessStatusCode)
            {
                // Step 1: Read the response body
                var responseBody = await response.Content.ReadAsStringAsync();

                // Step 2: Deserialize the response body into a user object (assuming AppIdentity contains user and role information)
                var loginResponse = JsonSerializer.Deserialize<AppIdentity>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Check if the login response is valid
                if (loginResponse == null)
                {
                    ModelState.AddModelError(string.Empty, "Login failed. Please try again.");
                    return View(loginRequest); // Return to login view if deserialization fails
                }

                // Step 3: Create a claims identity for the user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginResponse.UserName),
                    new Claim(ClaimTypes.Email, loginResponse.Email),
                    new Claim(ClaimTypes.Role, loginResponse.Rol) // Ensure Rol is correctly populated
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Step 4: Sign in the user and generate the authentication cookie
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false, // Set to true if you want the cookie to persist across sessions
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2) // Set cookie expiration
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                // Step 5: Redirect to the Mealboxes page after setting the authentication cookie
                return RedirectToAction("Mealboxes", "Mealbox");
            }
            else
            {
                // Handle failure (e.g., show an error message)
                ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your username and password.");
                return View(loginRequest); // Return to login view with error messages
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }


    }

}
