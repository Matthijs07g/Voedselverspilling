using Domain.Services;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BordspellenV3.Controllers
{
    public class TestController : Controller
    {
        private IGamesRepository gamesRepository;

        public TestController(IGamesRepository repo)
        {
            gamesRepository = repo;
        }

        public IActionResult Index() => View(gamesRepository.Games);
    }
}
