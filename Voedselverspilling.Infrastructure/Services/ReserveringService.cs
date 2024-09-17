using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.IRepositories;
using Voedselverspilling.Domain.Models;
using Voedselverspilling.DomainServices.Services;

namespace Voedselverspilling.Infrastructure.Services
{
    public class ReserveringService : IReserveringService
    {
        private readonly IReserveringRepository _reserveringRepository;

        public ReserveringService(IReserveringRepository reserveringRepository)
        {
            _reserveringRepository = reserveringRepository;
        }

        public async Task<Reservering> GetReserveringByIdAsync(int reserveringId)
        {
            return await _reserveringRepository.GetByIdAsync(reserveringId);
        }

        public async Task<IEnumerable<Reservering>> GetAllReserveringsAsync()
        {
            return await _reserveringRepository.GetAllAsync();
        }

        public async Task AddReserveringAsync(Reservering reservering)
        {
            await _reserveringRepository.AddAsync(reservering);
        }

        public async Task UpdateReserveringAsync(Reservering reservering)
        {
            await _reserveringRepository.UpdateAsync(reservering);
        }

        public async Task DeleteReserveringAsync(int reserveringId)
        {
            await _reserveringRepository.DeleteAsync(reserveringId);
        }
    }
}
