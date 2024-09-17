using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Application.Services;
using Voedselverspilling.Domain.IRepositories;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Infrastructure.Services
{
    public class PakketService : IPakketService
    {
        private readonly IPakketRepository _pakketRepository;

        public PakketService(IPakketRepository pakketRepository)
        {
            _pakketRepository = pakketRepository;
        }

        public async Task<Pakket> GetPakketByIdAsync(int pakketId)
        {
            return await _pakketRepository.GetByIdAsync(pakketId);
        }

        public async Task<IEnumerable<Pakket>> GetAllPakketsAsync()
        {
            return await _pakketRepository.GetAllAsync();
        }

        public async Task AddPakketAsync(Pakket pakket)
        {
            await _pakketRepository.AddAsync(pakket);
        }

        public async Task UpdatePakketAsync(Pakket pakket)
        {
            await _pakketRepository.UpdateAsync(pakket);
        }

        public async Task DeletePakketAsync(int pakketId)
        {
            await _pakketRepository.DeleteAsync(pakketId);
        }
    }
}
