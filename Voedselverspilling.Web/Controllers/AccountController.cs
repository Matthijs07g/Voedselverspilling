using Microsoft.AspNetCore.Mvc;
using Voedselverspilling.Web.Models;

namespace Voedselverspilling.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Net.Http;
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

        [HttpPost] // Change to HttpPost since it's for submitting a form
        public async Task<IActionResult> LoginUser(LoginRequest loginRequest)
        {
            // Convert the login request to JSON
            var jsonContent = JsonSerializer.Serialize(loginRequest);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");
 

            // Send the request to the API
            var response = await _httpClient.PostAsync($"{_apiBaseUrl}/Account/login", contentString);

            if (response.IsSuccessStatusCode)
            {
                
                return RedirectToAction("Mealboxes", "Mealbox");
            }
            else
            {
                // Handle failure (e.g., show an error message)
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                
                return RedirectToAction("Login", "Account");
            }
        }
    }

}
