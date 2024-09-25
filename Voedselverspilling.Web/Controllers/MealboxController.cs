using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.Web.Models;

namespace Voedselverspilling.Web.Controllers
{
    public class MealboxController : Controller
    {
        private readonly ILogger<MealboxController> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "http://localhost:5042/api/Pakket";

        public MealboxController(HttpClient httpClient, ILogger<MealboxController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }


        public async Task<IActionResult> MealboxesAsync()
        {
            List<Pakket> mealboxes = new List<Pakket>();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(_apiBaseUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    mealboxes = JsonSerializer.Deserialize<List<Pakket>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            catch (HttpRequestException e)
            {
                ModelState.AddModelError(string.Empty, $"Request error: {e.Message}");
            }

            // Transform `Pakket` to `MealboxModel` if needed
            var mealboxModels = mealboxes.Select(m => new MealboxModel
            {
                Id = m.Id,
                Naam = m.Naam,
                Stad = m.Stad,
                Prijs = m.Prijs,
                Type = m.Type,
                Is18 = m.Is18
            }).ToList();

            return View(mealboxModels);
        }

        public IActionResult MealboxAdd()
        {
            return View();
        }

        public async Task<IActionResult> MealboxDetails(int id)
        {
            // Fetch mealbox details from the API or database
            HttpResponseMessage response = await _httpClient.GetAsync($"{_apiBaseUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var mealbox = JsonSerializer.Deserialize<MealboxModel>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(mealbox);
            }

            return NotFound(); // Handle the case where the mealbox is not found
        }


        public IActionResult MealboxEdit()
        {
            return View();
        }

        public IActionResult MealboxDelete()
        {
            return View();
        }
    }
}
