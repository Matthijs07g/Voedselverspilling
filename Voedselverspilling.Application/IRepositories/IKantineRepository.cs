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
        Task<Kantine> AddAsync(Kantine kantine);
        Task<Kantine> UpdateAsync(Kantine kantine);
        Task DeleteAsync(int id);
    }
}
