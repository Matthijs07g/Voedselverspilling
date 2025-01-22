using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Voedselverspilling.DomainServices.IRepositories;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class PakketController : ControllerBase
    {
        private readonly IPakketRepository _pakketRepository;

        public PakketController(IPakketRepository pakketRepository)
        {
            _pakketRepository = pakketRepository;
        }


        //GET all
        [HttpGet]
        public async Task<IActionResult> GetAllPakketen()
        {
            IEnumerable<Pakket> Pakketen = await _pakketRepository.GetAllAsync();

            if (Pakketen == null)
            {
                return BadRequest("No Pakkets found");
            }
            else
            {
                return Ok(Pakketen);
            }
        }

        //GET one
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPakketById(int id)
        {
            Pakket Pakket = await _pakketRepository.GetByIdAsync(id);
            if (Pakket == null)
            {
                return BadRequest("Not Pakket found");
            }
            else
            {
                return Ok(Pakket);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPakket([FromBody] Pakket Pakket)
        {
            if (Pakket == null)
            {
                return BadRequest("Pakket object is null.");
            }

            await _pakketRepository.AddAsync(Pakket);
            return CreatedAtAction(nameof(AddPakket), new { id = Pakket.Id }, Pakket);
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePakket(int id, [FromBody] Pakket Pakket)
        {
            if (Pakket == null)
            {
                return BadRequest("Pakket object is null.");
            }

            Pakket.Id = id;

            await _pakketRepository.UpdateAsync(Pakket);
            return Ok(Pakket);
        }



        //DELETE
        [HttpDelete("{id}")]
        public async Task DeletePakket(int id)
        {
            await _pakketRepository.DeleteAsync(id);
        }
    }
}
