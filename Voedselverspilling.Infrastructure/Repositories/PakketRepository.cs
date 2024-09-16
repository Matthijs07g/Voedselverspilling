using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.Domain.Interfaces;
using Voedselverspilling.Domain.Models;

namespace Voedselverspilling.Infrastructure.Repositories
{
    public class PakketRepository : IPakketRepository
    {
        private readonly ApplicationDbContext _context;

        public PakketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Pakket> GetByIdAsync(int id)
        {
            return await _context.Pakketten.FindAsync(id);
        }

        public async Task<IEnumerable<Pakket>> GetAllAsync()
        {
            return await _context.Pakketten.ToListAsync();
        }

        public async Task AddAsync(Pakket Pakket)
        {
            _context.Pakketten.Add(Pakket);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Pakket Pakket)
        {
            _context.Entry(Pakket).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var Pakket = await _context.Pakketten.FindAsync(id);
            if (Pakket != null)
            {
                _context.Pakketten.Remove(Pakket);
                await _context.SaveChangesAsync();
            }
        }
    }
}
