using BordspellenV4.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BordspellenV4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController>? _logger;
        
        private IGamesRepository? gamesRepository;

        public HomeController(ILogger<HomeController> logger, IGamesRepository repo)
        {
            _logger = logger;
            gamesRepository = repo;
        }
        

        public IActionResult Index() => View(gamesRepository.Games);

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}