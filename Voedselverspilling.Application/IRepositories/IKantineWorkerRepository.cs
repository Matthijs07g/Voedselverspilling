using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.DomainServices.IRepositories
{
    public interface IKantineWorkerRepository
    {
        Task<KantineWorker> GetByIdAsync(int id);
        Task<IEnumerable<KantineWorker>> GetAllAsync();
        Task<KantineWorker> AddAsync(KantineWorker KantineWorker);
        Task<KantineWorker> UpdateAsync(KantineWorker KantineWorker);
        Task DeleteAsync(int id);
        Task<KantineWorker> GetByEmailAsync(string Email);
    }
}
