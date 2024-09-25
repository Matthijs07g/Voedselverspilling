﻿using Microsoft.AspNetCore.Mvc;
using Voedselverspilling.Domain.IRepositories;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        //POST
        //[HttpPost]
        //public async Task AddPakket(Pakket Pakket)
        //{
        //    await _pakketRepository.AddAsync(Pakket);
        //}

        //PUT
        //[HttpPut("{id}")]
        //public async Task UpdatePakket(int id, Pakket Pakket)
        //{
        //    await _pakketRepository.UpdateAsync(Pakket);
        //}

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
            if (Pakket == null || Pakket.Id != id)
            {
                return BadRequest("Pakket object is null or ID mismatch.");
            }

            await _pakketRepository.UpdateAsync(Pakket);
            return NoContent();
        }



        //DELETE
        [HttpDelete("{id}")]
        public async Task DeletePakket(int id)
        {
            await _pakketRepository.DeleteAsync(id);
        }
    }
}
