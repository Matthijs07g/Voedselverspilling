using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.DomainServices.Interfaces;
using Voedselverspilling.DomainServices.IRepositories;
using Voedselverspilling.Web.Models;

namespace Voedselverspilling.Web.Controllers
{
    [Authorize]
    public class MealboxController : Controller
    {
        private readonly ILogger<MealboxController> _logger;
        private readonly IPakketRepository _pakketRepository;
        private readonly IKantineRepository _kantineRepository;
        private readonly IKantineWorkerRepository _kantineWorkerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly UserManager<AppIdentity> _userManager;

        public MealboxController(
            ILogger<MealboxController> logger, 
            IPakketRepository pakketRepository, 
            IKantineRepository kantineRepository,
            IKantineWorkerRepository kantineWorkerRepository,
            IProductRepository productRepository,
            IStudentRepository studentRepository,
            UserManager<AppIdentity> userManager)
        {
            _logger = logger;
            _pakketRepository = pakketRepository;
            _kantineRepository = kantineRepository;
            _kantineWorkerRepository = kantineWorkerRepository;
            _productRepository = productRepository;
            _studentRepository = studentRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> MealboxesAsync()
        {
            var mealboxes = await _pakketRepository.GetAllAsync();
            var sortedMealboxes = mealboxes.OrderBy(m => m.EindDatum).ToList();

            return View(sortedMealboxes);
        }

        public async Task<IActionResult> MyMealboxesAsync()
        {
            var userIdentity = await _userManager.GetUserAsync(User);
            if(userIdentity == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var mealboxes = await _pakketRepository.GetByEmailAsync(userIdentity.Email);
            var sortedMealboxes = mealboxes.OrderBy(m => m.EindDatum).ToList();
            return View(sortedMealboxes);
        }

        [HttpGet]
        public async Task<IActionResult> MealboxAdd()
        {
            // Fetch available products from your data source
            var availableProducts = await _productRepository.GetAllAsync();
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
            mealboxCreateModel.AvailableProducts = await _productRepository.GetAllAsync();

            if(!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                return View(mealboxCreateModel);
            }

            var userIdentity = await _userManager.GetUserAsync(User);
            if (userIdentity == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var kantineWorker = await _kantineWorkerRepository.GetByEmailAsync(userIdentity.Email);
            var kantine = await _kantineRepository.GetByIdAsync(kantineWorker.KantineId);

            var selectedProducts = new List<Product>();
            foreach(var item in mealboxCreateModel.SelectedProductIds)
            {
                var product = await _productRepository.GetByIdAsync(item);
                if (product == null)
                {
                    ModelState.AddModelError(string.Empty, "Product not found");
                }

                selectedProducts.Add(product);
            }

            var newMealbox = new Pakket
            {
                Naam = mealboxCreateModel.Mealbox.Naam,
                Stad = kantine.Stad,
                Prijs = mealboxCreateModel.Mealbox.Prijs,
                KantineId = kantine.Id,
                Type = mealboxCreateModel.Mealbox.Type,
                Is18 = mealboxCreateModel.Mealbox.Is18,
                IsWarm = mealboxCreateModel.Mealbox.IsWarm,
                Producten = selectedProducts
            };

            try
            {
                await _pakketRepository.AddAsync(newMealbox);
                TempData["SuccessMessage"] = "Mealbox created successfully";
                return RedirectToAction("Mealboxes");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(mealboxCreateModel);
            }

        }

        [HttpGet]
        public async Task<IActionResult> MealboxDetails(int id)
        {
           var mealbox = await _pakketRepository.GetByIdAsync(id);
           if(mealbox == null)
            {
                return NotFound();
            }
            return View(mealbox);
        }

        [HttpGet]
        public async Task<IActionResult> MealboxEdit(int id)
        {
            var mealbox = await _pakketRepository.GetByIdAsync(id);
            var model = new MealboxEditModel
            {
                Mealbox = mealbox,
                AvailableProducts = await _productRepository.GetAllAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MealboxEdit(MealboxEditModel viewModel)
        {
            var mealbox = await _pakketRepository.GetByIdAsync(viewModel.Mealbox.Id);
            if (mealbox == null)
            {
                return NotFound();
            }

            mealbox.Naam = viewModel.Mealbox.Naam;
            mealbox.Prijs = viewModel.Mealbox.Prijs;
            mealbox.Type = viewModel.Mealbox.Type;
            mealbox.Is18 = viewModel.Mealbox.Is18;
            mealbox.IsWarm = viewModel.Mealbox.IsWarm;
            mealbox.EindDatum = viewModel.Mealbox.EindDatum;

            var userIdentity = await _userManager.GetUserAsync(User);
            if (userIdentity == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var kantineWorker = await _kantineWorkerRepository.GetByEmailAsync(userIdentity.Email);
            var kantine = await _kantineRepository.GetByIdAsync(kantineWorker.KantineId);

            if(!kantine.IsWarm && mealbox.IsWarm)
            {
                ModelState.AddModelError(string.Empty, "Kantine does not serve warm food");
                return View(viewModel);
            }

            var selectedProducts = new List<Product>();
            foreach (var item in viewModel.SelectedProductIds)
            {
                var product = await _productRepository.GetByIdAsync(item);
                if (product == null)
                {
                    ModelState.AddModelError(string.Empty, "Product not found");
                }

                selectedProducts.Add(product);
            }
            mealbox.Producten = selectedProducts;

            await _pakketRepository.UpdateAsync(mealbox);
            TempData["SuccessMessage"] = "Mealbox updated successfully";
            return RedirectToAction("Mealboxes");
        }

        public async Task<IActionResult> MealboxDelete(int id)
        {
            var mealbox = await _pakketRepository.GetByIdAsync(id);
            if (mealbox == null)
            {
                return NotFound();
            }
            if(mealbox.ReservedBy != null)
            {
                ModelState.AddModelError(string.Empty, "Mealbox is reserved and cannot be deleted");
                return RedirectToAction("Mealboxes");
            }
            await _pakketRepository.DeleteAsync(id);
            TempData["SuccessMessage"] = "Mealbox deleted successfully";
            return RedirectToAction("Mealboxes");
        }

        public async Task<IActionResult> MealboxReserveer(int id)
        {
            var userIdentity = await _userManager.GetUserAsync(User);
            if (userIdentity == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var student = await _studentRepository.GetByEmailAsync(userIdentity.Email);

            await _pakketRepository.ReservePakketAsync(id, student);
            TempData["SuccessMessage"] = "Mealbox reserved successfully";
            return RedirectToAction("MyMealboxes");
        }
    }
}
