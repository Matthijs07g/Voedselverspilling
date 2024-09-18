using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Voedselverspilling.Domain.IRepositories;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KantineController : Controller
    {
        private readonly IKantineRepository _kantineRepository;

        public KantineController(IKantineRepository kantineRepository)
        {
            _kantineRepository = kantineRepository;
        }

        //GET all
        [HttpGet]
        public async Task<IActionResult> GetAllKantines()
        {
            IEnumerable<Kantine> kantines = await _kantineRepository.GetAllAsync();

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
            Kantine kantine = await _kantineRepository.GetByIdAsync(id);
            if(kantine == null)
            {
                return BadRequest("Not kantine found");
            }
            else
            {
                return Ok(kantine);
            }
        }

        //POST
        //[HttpPost]
        //public async Task AddKantine(Kantine kantine)
        //{
        //    await _kantineRepository.AddAsync(kantine);
        //}

        //PUT
        //[HttpPut("{id}")]
        //public async Task UpdateKantine(int id, Kantine kantine)
        //{
        //    await _kantineRepository.UpdateAsync(kantine);
        //}

        [HttpPost]
        public async Task<IActionResult> AddKantine([FromBody] Kantine kantine)
        {
            if (kantine == null)
            {
                return BadRequest("Kantine object is null.");
            }

            await _kantineRepository.AddAsync(kantine);
            return CreatedAtAction(nameof(AddKantine), new { id = kantine.Id }, kantine);
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKantine(int id, [FromBody] Kantine kantine)
        {
            if (kantine == null || kantine.Id != id)
            {
                return BadRequest("Kantine object is null or ID mismatch.");
            }

           await _kantineRepository.UpdateAsync(kantine);
           return NoContent();
        }

        

        //DELETE
        [HttpDelete("{id}")]
        public async Task DeleteKantine(int id)
        {
            await _kantineRepository.DeleteAsync(id);
        }
    }
}
