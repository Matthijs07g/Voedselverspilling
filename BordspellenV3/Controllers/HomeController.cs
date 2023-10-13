using Domain.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace BordspellenV3.Controllers
{
    public class HomeController : Controller
    {
        private IGamesRepository _gamesRepository;

        
        
        
        /*
        private BordspellenDbContext context;

        public HomeController(BordspellenDbContext dbContext)
        {
            this.context = dbContext;
        }

        public IActionResult Index([FromQuery] string selectedGame)
        {
            return View()
        }*/

        
        public HomeController(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository;
        }

        public IActionResult Index() => View(_gamesRepository.Games);
        
    }
}
