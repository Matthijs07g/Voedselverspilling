using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Application.Services;
using Voedselverspilling.Domain.Interfaces;
using Voedselverspilling.Domain.IRepositories;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Infrastructure.Services
{
    public class KantineService : IKantineService
    {
        private readonly IKantineRepository _kantineRepository;

        public KantineService(IKantineRepository kantineRepository)
        {
            _kantineRepository = kantineRepository;
        }

        public async Task<Kantine> GetKantineByIdAsync(int kantineId)
        {
            return await _kantineRepository.GetByIdAsync(kantineId);
        }

        public async Task<IEnumerable<Kantine>> GetAllKantinesAsync()
        {
            return await _kantineRepository.GetAllAsync();
        }

        public async Task AddKantineAsync(Kantine kantine)
        {
            await _kantineRepository.AddAsync(kantine);
        }

        public async Task UpdateKantineAsync(Kantine kantine)
        {
            await _kantineRepository.UpdateAsync(kantine);
        }

        public async Task DeleteKantineAsync(int kantineId)
        {
            await _kantineRepository.DeleteAsync(kantineId);
        }
    }
}
