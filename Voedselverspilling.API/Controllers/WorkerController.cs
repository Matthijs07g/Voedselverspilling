using Microsoft.AspNetCore.Mvc;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.DomainServices.Services;

namespace Voedselverspilling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkerController : Controller
    {
        private readonly IKantineWorkerService _kantineWorkerService;

        public WorkerController(IKantineWorkerService kantineWorkerService)
        {
            _kantineWorkerService = kantineWorkerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorker()
        {
            IEnumerable<KantineWorker> workers = await _kantineWorkerService.GetAllKantineWorkersAsync();

            if (workers == null)
            {
                return BadRequest("No Workers found");
            }
            else
            {
                return Ok(workers);
            }
        }

        //GET one
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReserveringById(int id)
        {
            KantineWorker worker = await _kantineWorkerService.GetKantineWorkerByIdAsync(id);
            if (worker == null)
            {
                return BadRequest("Not Worker found");
            }
            else
            {
                return Ok(worker);
            }
        }
    }
}
