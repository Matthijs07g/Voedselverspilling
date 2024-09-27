using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voedselverspilling.Application.Services;
using Voedselverspilling.Domain.IRepositories;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KantineController : ControllerBase
    {
        private readonly IKantineService _kantineService;

        public KantineController(IKantineService kantineService)
        {
            _kantineService = kantineService;
        }


        //GET all
        [HttpGet]
        public async Task<IActionResult> GetAllKantines()
        {
            IEnumerable<Kantine> kantines = await _kantineService.GetAllKantinesAsync();

            if(kantines == null)
            {
                return BadRequest("No kantines found");
            }
            else
            {
                return Ok(kantines);
            }
        }

        //GET one
        [HttpGet("{id}")]
        public async Task<IActionResult> GetKantineById(int id)
        {
            Kantine kantine = await _kantineService.GetKantineByIdAsync(id);
            if(kantine == null)
            {
                return BadRequest("Not kantine found");
            }
            else
            {
                return Ok(kantine);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddKantine([FromBody] Kantine kantine)
        {
            if (kantine == null)
            {
                return BadRequest("Kantine object is null.");
            }

            await _kantineService.AddKantineAsync(kantine);
            return CreatedAtAction(nameof(AddKantine), new { id = kantine.Id }, kantine);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKantine(int id, [FromBody] Kantine kantine)
        {
            if (kantine == null)
            {
                return BadRequest("Kantine object is null.");
            }

            kantine.Id = id;

           await _kantineService.UpdateKantineAsync(kantine);
           return Ok(kantine);
        }

        

        //DELETE
        [HttpDelete("{id}")]
        public async Task DeleteKantine(int id)
        {
            await _kantineService.DeleteKantineAsync(id);
        }
    }
}
