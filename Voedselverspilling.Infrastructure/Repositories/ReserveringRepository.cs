using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.IRepositories;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Infrastructure.Repositories
{
    public class ReserveringRepository : IReserveringRepository
    {
        private readonly ApplicationDbContext _context;

        public ReserveringRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Reservering> GetByIdAsync(int id)
        {
            return await _context.Reserveringen.FindAsync(id);
        }

        public async Task<IEnumerable<Reservering>> GetAllAsync()
        {
            return await _context.Reserveringen.ToListAsync();
        }

        public async Task AddAsync(Reservering Reservering)
        {
            _context.Reserveringen.Add(Reservering);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reservering Reservering)
        {
            _context.Entry(Reservering).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var Reservering = await _context.Reserveringen.FindAsync(id);
            if (Reservering != null)
            {
                _context.Reserveringen.Remove(Reservering);
                await _context.SaveChangesAsync();
            }
        }
    }
}
