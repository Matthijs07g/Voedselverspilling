using Microsoft.AspNetCore.Mvc;
using Voedselverspilling.Web.Models;

namespace Voedselverspilling.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Optionally, you can create a Logout method
        [HttpPost]
        public IActionResult Logout()
        {
            // Clear the user session/cookies
            return RedirectToAction("Login");
        }
    }
}
