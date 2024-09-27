using Microsoft.AspNetCore.Authorization;
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
        private readonly string _apiBaseUrl = "http://localhost:5042/api";

        public MealboxController(HttpClient httpClient, ILogger<MealboxController> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        [Authorize(Roles = "Admin, Worker")]
        public async Task<IActionResult> MealboxesAsync()
        {
            List<Pakket> mealboxes = new List<Pakket>();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_apiBaseUrl}/Pakket");

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
            HttpResponseMessage response = await _httpClient.GetAsync($"{_apiBaseUrl}/Pakket/{id}");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Succesful get pakket");
                string responseBody = await response.Content.ReadAsStringAsync();
                var mealbox = JsonSerializer.Deserialize<MealboxModel>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                List<ProductModel> products = new List<ProductModel>();
                foreach (var productId in mealbox.ProductenId)
                {
                    HttpResponseMessage message = await _httpClient.GetAsync($"{_apiBaseUrl}/Product/{productId}");
                    Console.WriteLine("Getting product");
                    if (message.IsSuccessStatusCode)
                    {
                        string productBody = await message.Content.ReadAsStringAsync(); // Read the response for the product
                        var product = JsonSerializer.Deserialize<ProductModel>(productBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        Console.WriteLine($"Product: {product}");
                        if (product != null)
                        {
                            products.Add(product);
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                }

                MealboxDetailModel mealboxDetailModel = new MealboxDetailModel()
                {
                    MealboxModel = mealbox,
                    Products = products
                };

                return View(mealboxDetailModel);
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
