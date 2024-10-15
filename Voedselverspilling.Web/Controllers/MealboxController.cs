using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
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
            if (User.IsInRole("Worker"))
            {
                KantineWorker kantineWorker = await GetLoggedWorker();
                foreach (var item in mealboxModels)
                {
                    if(item.KantineId != kantineWorker.KantineId)
                    {
                        openMealboxes.Add(item);
                    }
                }
            }
            else
            {
                foreach (var mealbox in mealboxModels)
                {
                    if (mealbox.GereserveerdDoor == null)
                    {
                        openMealboxes.Add(mealbox);
                    }
                }
            }

            return View(openMealboxes);
        }

        public async Task<IActionResult> MyMealboxesAsync()
        {
            

            var mealboxModels = GetAllMealboxes();
            
            List<MealboxModel> result = await mealboxModels;
            List<MealboxModel> myMealboxes = new List<MealboxModel>();

            if (User.IsInRole("Worker"))
            {
                KantineWorker kantineWorker = await GetLoggedWorker();
                foreach (var item in result)
                {
                    if(item.KantineId == kantineWorker.KantineId)
                    {
                        myMealboxes.Add(item);
                    }
                }
            }
            if (User.IsInRole("Student"))
            {
                Student student = await GetLoggedStudent();

                if (student != null)
                {
                    foreach (var item in result)
                    {
                        if (item.GereserveerdDoor == student.Id)
                        {

                            myMealboxes.Add(item);
                        }
                    }
                }
            }
            return View(myMealboxes);
        }

        [HttpGet]
        public async Task<IActionResult> MealboxAdd()
        {
            // Fetch available products from your data source
            var availableProducts = await GetAvailableProducts();
            Console.WriteLine("Loading products for creating");

            var model = new MealboxCreateModel
            {
                AvailableProducts = availableProducts
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MealboxAdd(MealboxCreateModel mealboxCreateModel)
        {
            Console.WriteLine("Starting the creating of pakket");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model state is invalid:");
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        Console.WriteLine($"Key: {modelState.Key}, Error: {error.ErrorMessage}");
                    }
                }
                ModelState.AddModelError(string.Empty, "Failed to create the mealbox. Please try again.");
                // If the model is invalid, return to the view with the current model data
                mealboxCreateModel.AvailableProducts = await GetAvailableProducts();
                return View(mealboxCreateModel);
            }

            KantineWorker worker = await GetLoggedWorker();

            HttpResponseMessage kantine = await _httpClient.GetAsync($"{_apiBaseUrl}/kantine/{worker.KantineId}");
            if (kantine.IsSuccessStatusCode)
            {

                string responseBody = await kantine.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Kantine>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


                if (mealboxCreateModel.Mealbox.IsWarm == true && result.IsWarm == false)
                {
                    ModelState.AddModelError(string.Empty, "Kan geen warm eten serveren.");
                    mealboxCreateModel.AvailableProducts = await GetAvailableProducts();
                    return View(mealboxCreateModel);
                }
            }

            // Create new pakket object from the model
            var newPakket = new Pakket
            {
                Naam = mealboxCreateModel.Mealbox.Naam,
                Stad = worker.Stad,
                Prijs = mealboxCreateModel.Mealbox.Prijs,
                KantineId = worker.KantineId,
                Type = mealboxCreateModel.Mealbox.Type,
                Is18 = mealboxCreateModel.Mealbox.Is18,
                IsWarm = mealboxCreateModel.Mealbox.IsWarm,
                ProductenId = mealboxCreateModel.SelectedProductIds // Use selected product IDs
            };

            // Serialize and send the new pakket to your API for creation
            var jsonPakket = JsonSerializer.Serialize(newPakket);
            var content = new StringContent(jsonPakket, Encoding.UTF8, "application/json");

            Console.WriteLine(content);

            HttpResponseMessage response = await _httpClient.PostAsync($"{_apiBaseUrl}/Pakket", content);
            Console.WriteLine("Creating pakket");

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Succesful in creating pakket");
                return RedirectToAction("Mealboxes"); // Redirect after successful creation
            }

            ModelState.AddModelError(string.Empty, "Failed to create the pakket. Please try again.");
            mealboxCreateModel.AvailableProducts = await GetAvailableProducts(); // Fetch products again for the view
            return View(mealboxCreateModel); // Return to the view with the current model
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

                ReserveringModel reservering = await GetReserveringByPakketId(id);
                Student student = await GetLoggedStudent();
                if(reservering != null && student.Id == reservering.StudentId)
                {
                    mealbox.GereserveerdDoor = student.Id;
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

        public async Task<IActionResult> MealboxEdit(int id)
        {
            // Fetch the mealbox data
            HttpResponseMessage response = await _httpClient.GetAsync($"{_apiBaseUrl}/Pakket/{id}");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var mealbox = JsonSerializer.Deserialize<MealboxModel>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (mealbox != null)
                {
                    // Use the GetAvailableProducts method to get the list of products
                    var availableProducts = await GetAvailableProducts();

                    var mealboxEditViewModel = new MealboxEditModel
                    {
                        Mealbox = mealbox,
                        AvailableProducts = availableProducts, // Pass the list of all products
                        SelectedProductIds = mealbox.ProductenId // Preselect the current products
                    };
                    return View(mealboxEditViewModel); // Pass both mealbox data and available products
                }
            }

            return NotFound(); // If mealbox or products not found, return 404
        }

        [HttpPost]
        public async Task<IActionResult> MealboxEdit(MealboxEditModel viewModel)
        {
            var reservering = await GetReserveringByPakketId(viewModel.Mealbox.Id);
            if (reservering == null)
            {
                Console.WriteLine("Starting the creating of pakket");
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("Model state is invalid:");
                    foreach (var modelState in ModelState)
                    {
                        foreach (var error in modelState.Value.Errors)
                        {
                            Console.WriteLine($"Key: {modelState.Key}, Error: {error.ErrorMessage}");
                        }
                    }
                    ModelState.AddModelError(string.Empty, "Failed to create the mealbox. Please try again.");
                    // If the model is invalid, return to the view with the current model data
                    viewModel.AvailableProducts = await GetAvailableProducts();
                    return View(viewModel);
                }

                KantineWorker worker = await GetLoggedWorker();

                HttpResponseMessage kantine = await _httpClient.GetAsync($"{_apiBaseUrl}/kantine/{worker.KantineId}");
                if (kantine.IsSuccessStatusCode) {

                    string responseBody = await kantine.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<Kantine>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


                    if (viewModel.Mealbox.IsWarm == true && result.IsWarm == false)
                    {
                        ModelState.AddModelError(string.Empty, "Kan geen warm eten serveren/");
                        viewModel.AvailableProducts = await GetAvailableProducts();
                        return View(viewModel);
                    }
                }

                // Create new pakket object from the model
                var newPakket = new Pakket
                {
                    Naam = viewModel.Mealbox.Naam,
                    Stad = worker.Stad,
                    Prijs = viewModel.Mealbox.Prijs,
                    KantineId = worker.KantineId,
                    Type = viewModel.Mealbox.Type,
                    Is18 = viewModel.Mealbox.Is18,
                    IsWarm = viewModel.Mealbox.IsWarm,
                    ProductenId = viewModel.SelectedProductIds // Use selected product IDs
                };

                // Serialize and send the new pakket to your API for creation
                var jsonPakket = JsonSerializer.Serialize(newPakket);
                var content = new StringContent(jsonPakket, Encoding.UTF8, "application/json");

                // Send the PUT request to update the mealbox
                HttpResponseMessage response = await _httpClient.PutAsync($"{_apiBaseUrl}/Pakket/{viewModel.Mealbox.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Mealboxes"); // Redirect back to the list of mealboxes after successful update
                }

                ModelState.AddModelError(string.Empty, "Failed to update the mealbox. Please try again.");
                // Fetch the available products again in case of failure
                viewModel.AvailableProducts = await GetAvailableProducts();
                return View(viewModel); // If API call fails, return to the form
            }
            ModelState.AddModelError(string.Empty, "Can't edit a Pakket that has a reservering");
            return View(viewModel);
        }

        public async Task<IActionResult> MealboxDelete(int id)
        {
            var reservering = await GetReserveringByPakketId(id);
            if (reservering == null)
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/Pakket/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Mealboxes");
                }

                ModelState.AddModelError(string.Empty, "Failed to delete the mealbox. Try again");
                return await MealboxDetails(id);
            }
            ModelState.AddModelError(string.Empty, "Failed to delete the mealbox. Already has a reservering");
            return RedirectToAction("Mealboxes");
        }

        public async Task<IActionResult> MealboxReserveer(int id)
        {
            Console.WriteLine("Starting reserveren process");
            
            var reservering = await GetReserveringByPakketId(id);
            if(reservering != null)
            {
                Console.WriteLine("Error reservering bestaat al");
                ModelState.AddModelError(string.Empty, "Pakket heeft al een reservering");
                return RedirectToAction("MyMealboxes");
            }
            else
            {
                Student student = await GetLoggedStudent();

                Reservering newReservering = new Reservering
                {
                    ReservaringDatum = new DateTime(),
                    IsOpgehaald = false,
                    TijdOpgehaald = new DateTime().AddDays(3),
                    StudentId = student.Id,
                    PakketId = id,
                };

                var jsonPakket = JsonSerializer.Serialize(newReservering);
                var content = new StringContent(jsonPakket, Encoding.UTF8, "application/json");

                Request.Cookies.TryGetValue("jwt", out var jwt);
                Console.WriteLine($"JWT: {jwt}");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                Console.WriteLine("Creating reservering");
                HttpResponseMessage response = await _httpClient.PostAsync($"{_apiBaseUrl}/Reservering", content);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("Unauthorized acces");
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("MyMealboxes");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return RedirectToAction("MyMealboxes");
                }
            }
        }

        public async Task<IActionResult> CancelReservation(MealboxDetailModel model)
        {
            ReserveringModel reservering = await GetReserveringByPakketId(model.MealboxModel.Id);
            if (reservering == null)
            {
                ModelState.AddModelError(string.Empty, "Reservering niet gevonden");
                return View(model);
            }
            else
            {
                Request.Cookies.TryGetValue("jwt", out var jwt);
                Console.WriteLine($"JWT: {jwt}");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                HttpResponseMessage response = await _httpClient.DeleteAsync($"{_apiBaseUrl}/Reservering/{reservering.ReserveringId}");

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("Unauthorized acces");
                }

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("MyMealboxes");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View(model);
                }
            }
        }




        private async Task<List<MealboxModel>> GetAllMealboxes()
        {
            List<Pakket> mealboxes = new List<Pakket>();
            List<Reservering> reserverings = new List<Reservering>();
            Student student = await GetLoggedStudent();
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
                KantineId = m.KantineId,
                Prijs = m.Prijs,
                Type = m.Type,
                Is18 = m.Is18,
                IsWarm = m.IsWarm,
            }).ToList();

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            DateOnly alocohol = today.AddYears(-18);


            foreach (var mealbox in mealboxModels)
            {
                
                foreach (var reservering in reserveringModels)
                {
                    if (mealbox.Id == reservering.PakketId)
                    {
                        mealbox.GereserveerdDoor = reservering.StudentId;
                        mealbox.OphaalTijd = reservering.TijdOpgehaald;
                    }
                }
            }
            

            return mealboxModels;
        }
    
        private async Task<Student> GetLoggedStudent()
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

            foreach (var student in students)
            {
                if(student.Emailaddress == User.Identity.Name)
                {
                    return student;
                }
            }
            return null;

        }

        private async Task<KantineWorker> GetLoggedWorker()
        {
            List<KantineWorker> workers = new List<KantineWorker>();

            try
            {
                Request.Cookies.TryGetValue("jwt", out var jwt);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                HttpResponseMessage response = await _httpClient.GetAsync($"{_apiBaseUrl}/Worker");

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Console.WriteLine("Unauthorized acces");
                }

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    workers = JsonSerializer.Deserialize<List<KantineWorker>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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

            foreach (var worker in workers)
            {
                if (worker.Email == User.Identity.Name)
                {
                    return worker;
                }
            }
            return null;

        }

        private async Task<List<ProductModel>> GetAvailableProducts()
        {
            List<ProductModel> products = new List<ProductModel>();

            try
            {
                // Stuur een GET-verzoek naar de API om de producten op te halen
                HttpResponseMessage response = await _httpClient.GetAsync($"{_apiBaseUrl}/Product");

                if (response.IsSuccessStatusCode)
                {
                    // Lees de inhoud van de response
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // Deserialize de JSON-response naar een lijst van ProductModel
                    products = JsonSerializer.Deserialize<List<ProductModel>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    // Hier kun je eventueel foutafhandeling doen, bijvoorbeeld loggen
                    ModelState.AddModelError(string.Empty, "Fout bij het ophalen van de producten.");
                }
            }
            catch (HttpRequestException e)
            {
                // Foutafhandeling voor netwerkproblemen
                ModelState.AddModelError(string.Empty, $"Fout bij het uitvoeren van de aanvraag: {e.Message}");
            }

            return products; // Geef de lijst van producten terug
        }

        private async Task<ReserveringModel?>? GetReserveringByPakketId(int pakketId)
        {
            List<ReserveringModel> reserverings = new List<ReserveringModel>();
            HttpResponseMessage response = await _httpClient.GetAsync($"{_apiBaseUrl}/Reservering");
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                reserverings = JsonSerializer.Deserialize<List<ReserveringModel>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                // Hier kun je eventueel foutafhandeling doen, bijvoorbeeld loggen
                ModelState.AddModelError(string.Empty, "Fout bij het ophalen van de producten.");
            }

            foreach (var reservering in reserverings)
            {
                if(reservering.PakketId == pakketId)
                {
                    return reservering;
                }
            }
            return null;


        }
    
    }
}
