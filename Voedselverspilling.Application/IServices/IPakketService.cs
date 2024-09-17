using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Application.Services
{
    public interface IPakketService
    {
        Task<Pakket> GetPakketByIdAsync(int pakketId);
        Task<IEnumerable<Pakket>> GetAllPakketsAsync();
        Task AddPakketAsync(Pakket pakket);
        Task UpdatePakketAsync(Pakket pakket);
        Task DeletePakketAsync(int pakketId);
    }
}
