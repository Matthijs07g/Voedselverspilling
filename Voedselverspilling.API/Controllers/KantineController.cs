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
        public async Task<IEnumerable<Kantine>> GetAllKantines()
        {
            return await _kantineRepository.GetAllAsync();
        }

        //GET one
        [HttpGet("{id}")]
        public async Task<Kantine> GetKantineById(int id)
        {
            return await _kantineRepository.GetByIdAsync(id);
        }

        //POST
        [HttpPost]
        public async Task AddKantine(Kantine kantine)
        {
            await _kantineRepository.AddAsync(kantine);
        }

        //PUT
        [HttpPut("{id}")]
        public async Task UpdateKantine(int id, Kantine kantine)
        {
            await _kantineRepository.UpdateAsync(kantine);
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task DeleteKantine(int id)
        {
            await _kantineRepository.DeleteAsync(id);
        }
    }
}
