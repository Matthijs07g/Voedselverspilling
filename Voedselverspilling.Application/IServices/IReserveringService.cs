using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.DomainServices.Services
{
    public interface IReserveringService
    {
        Task<Reservering> GetReserveringByIdAsync(int reserveringId);
        Task<IEnumerable<Reservering>> GetAllReserveringsAsync();
        Task AddReserveringAsync(Reservering reservering);
        Task UpdateReserveringAsync(Reservering reservering);
        Task DeleteReserveringAsync(int reserveringId);
    }
}
