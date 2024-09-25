using Microsoft.AspNetCore.Mvc;

namespace Voedselverspilling.API.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
