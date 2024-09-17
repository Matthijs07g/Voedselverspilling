using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.Interfaces;
using Voedselverspilling.Domain.IRepositories;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.DomainServices.Services;

namespace Voedselverspilling.Infrastructure.Services
{
    public class KantineWorkerService : IKantineWorkerService
    {
        private readonly IKantineWorkerRepository _kantineWorkerRepository;

        public KantineWorkerService(IKantineWorkerRepository kantineWorkerRepository)
        {
            _kantineWorkerRepository = kantineWorkerRepository;
        }

        public async Task<KantineWorker> GetKantineWorkerByIdAsync(int kantineWorkerId)
        {
            return await _kantineWorkerRepository.GetByIdAsync(kantineWorkerId);
        }

        public async Task<IEnumerable<KantineWorker>> GetAllKantineWorkersAsync()
        {
            return await _kantineWorkerRepository.GetAllAsync();
        }

        public async Task AddKantineWorkerAsync(KantineWorker kantineWorker)
        {
            await _kantineWorkerRepository.AddAsync(kantineWorker);
        }

        public async Task UpdateKantineWorkerAsync(KantineWorker kantineWorker)
        {
            await _kantineWorkerRepository.UpdateAsync(kantineWorker);
        }

        public async Task DeleteKantineWorkerAsync(int kantineWorkerId)
        {
            await _kantineWorkerRepository.DeleteAsync(kantineWorkerId);
        }
    }
}
