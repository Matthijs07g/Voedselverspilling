using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Application.Services
{
    public interface IKantineService
    {
        Task<Kantine> GetKantineByIdAsync(int kantineId);
        Task<IEnumerable<Kantine>> GetAllKantinesAsync();
        Task AddKantineAsync(Kantine kantine);
        Task UpdateKantineAsync(Kantine kantine);
        Task DeleteKantineAsync(int kantineId);
    }
}
