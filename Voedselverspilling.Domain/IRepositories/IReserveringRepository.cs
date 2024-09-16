using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Domain.IRepositories
{
    public interface IReserveringRepository
    {
        Task<Reservering> GetByIdAsync(int id);
        Task<IEnumerable<Reservering>> GetAllAsync();
        Task AddAsync(Reservering reservering);
        Task UpdateAsync(Reservering reservering);
        Task DeleteAsync(int id);
    }
}
