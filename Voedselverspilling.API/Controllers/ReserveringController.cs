using Microsoft.AspNetCore.Mvc;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.DomainServices.Services;
using Voedselverspilling.Infrastructure.Services;

namespace Voedselverspilling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReserveringController : Controller
    {
        private readonly IReserveringService _reserveringService;

        public ReserveringController (IReserveringService reserveringService)
        {
            _reserveringService = reserveringService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReserveringen()
        {
            IEnumerable<Reservering> reserverings = await _reserveringService.GetAllReserveringsAsync();

            if (reserverings == null)
            {
                return BadRequest("No Reserveringen found");
            }
            else
            {
                return Ok(reserverings);
            }
        }

        //GET one
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReserveringById(int id)
        {
            Reservering reservering = await _reserveringService.GetReserveringByIdAsync(id);
            if (reservering == null)
            {
                return BadRequest("Not Reservering found");
            }
            else
            {
                return Ok(reservering);
            }
        }
    }
}
