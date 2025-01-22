using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.DomainServices.IRepositories
{
    public interface IKantineRepository
    {
        Task<Kantine> GetByIdAsync(int id);
        Task<IEnumerable<Kantine>> GetAllAsync();
        Task AddAsync(Kantine kantine);
        Task UpdateAsync(Kantine kantine);
        Task DeleteAsync(int id);
    }
}
