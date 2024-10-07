using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.Web.Models;

namespace Voedselverspilling.Web.Controllers
{
    [Authorize]
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

        
        public async Task<IActionResult> MealboxesAsync()
        {
            List<Pakket> mealboxes = new List<Pakket>();

            var mealboxModels = await GetAllMealboxes();

            List<MealboxModel> openMealboxes = new List<MealboxModel>();

            foreach (var mealbox in mealboxModels)
            {
                if(mealbox.GereserveerdDoor == null)
                {
                    openMealboxes.Add(mealbox);
                }                
            }

            return View(openMealboxes);
        }

        public async Task<IActionResult> MyMealboxesAsync()
        {
            List<Student> students = new List<Student>();

            try
            {
                Console.WriteLine("Getting pakketen");
                Request.Cookies.TryGetValue("jwt", out var jwt);
                Console.WriteLine($"JWT: {jwt}");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                HttpResponseMessage response = await _httpClient.GetAsync($"{_apiBaseUrl}/Student");

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("Unauthorized acces");
                }

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    students = JsonSerializer.Deserialize<List<Student>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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

            var mealboxModels = GetAllMealboxes();
            
            List<MealboxModel> result = await mealboxModels;
            List<MealboxModel> myMealboxes = new List<MealboxModel>();

            foreach (var student in students)
            {
                if (student.Emailaddress == User.Identity.Name)
                {
                    foreach (var item in result)
                    {
                        if(item.GereserveerdDoor == student.Id)
                        {
                            myMealboxes.Add(item);
                        }
                    }
                }
            }
            return View(myMealboxes);
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




        private async Task<List<MealboxModel>> GetAllMealboxes()
        {
            List<Pakket> mealboxes = new List<Pakket>();
            List<Reservering> reserverings = new List<Reservering>();

            try
            {
                Console.WriteLine("Getting pakketen");
                Request.Cookies.TryGetValue("jwt", out var jwt);
                Console.WriteLine($"JWT: {jwt}");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                HttpResponseMessage response = await _httpClient.GetAsync($"{_apiBaseUrl}/Pakket");

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("Unauthorized acces");
                }

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    mealboxes = JsonSerializer.Deserialize<List<Pakket>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }

                HttpResponseMessage responseReservering = await _httpClient.GetAsync($"{_apiBaseUrl}/Reservering");

                if (responseReservering.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("Unauthorized acces");
                }

                if (responseReservering.IsSuccessStatusCode)
                {
                    string responseBodyReservering = await responseReservering.Content.ReadAsStringAsync();
                    reserverings = JsonSerializer.Deserialize<List<Reservering>>(responseBodyReservering, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
            var reserveringModels = reserverings.Select(r => new ReserveringModel
            {
                ReserveringId = r.ReserveringId,
                ReservaringDatum = r.ReservaringDatum,
                IsOpgehaald = r.IsOpgehaald,
                TijdOpgehaald = r.TijdOpgehaald,
                StudentId = r.StudentId,
                PakketId = r.PakketId,
            }).ToList();

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

            foreach (var mealbox in mealboxModels)
            {
                foreach (var reservering in reserveringModels)
                {
                    if (mealbox.Id == reservering.PakketId)
                    {
                        mealbox.GereserveerdDoor = reservering.StudentId;
                    }
                }
            }
            return mealboxModels;
        }
    }
}
