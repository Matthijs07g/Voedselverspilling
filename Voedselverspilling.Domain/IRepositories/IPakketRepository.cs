using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Domain.Interfaces
{
    public interface IPakketRepository
    {
        Task<Pakket> GetByIdAsync(int id);
        Task<IEnumerable<Pakket>> GetAllAsync();
        Task AddAsync(Pakket pakket);
        Task UpdateAsync(Pakket pakket);
        Task DeleteAsync(int id);
    }
}
