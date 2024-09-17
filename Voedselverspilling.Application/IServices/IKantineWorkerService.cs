using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.DomainServices.Services
{
    public interface IKantineWorkerService
    {
        Task<KantineWorker> GetKantineWorkerByIdAsync(int kantineWorkerId);
        Task<IEnumerable<KantineWorker>> GetAllKantineWorkersAsync();
        Task AddKantineWorkerAsync(KantineWorker kantineWorker);
        Task UpdateKantineWorkerAsync(KantineWorker kantineWorker);
        Task DeleteKantineWorkerAsync(int kantineWorkerId);
    }
}
