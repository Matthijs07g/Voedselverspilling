using Microsoft.AspNetCore.Mvc;
using Voedselverspilling.Application.Services;
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

        [HttpPost]
        public async Task<IActionResult> AddReservering([FromBody] Reservering reservering)
        {
            if (reservering == null)
            {
                return BadRequest("Reservering object is null.");
            }

            await _reserveringService.AddReserveringAsync(reservering);
            return CreatedAtAction(nameof(reservering), new { id = reservering.ReserveringId }, reservering);
        }

        [HttpDelete("{id}")]
        public async Task DeleteReservering(int id)
        {
            await _reserveringService.DeleteReserveringAsync(id);
        }
    }
}
