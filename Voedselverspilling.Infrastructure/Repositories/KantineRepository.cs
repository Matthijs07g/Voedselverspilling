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
    public class KantineRepository : IKantineRepository
    {
        private readonly ApplicationDbContext _context;

        public KantineRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Kantine> GetByIdAsync(int id)
        {
            return await _context.Kantines.FindAsync(id);
        }

        public async Task<IEnumerable<Kantine>> GetAllAsync()
        {
            return await _context.Kantines.ToListAsync();
        }

        public async Task AddAsync(Kantine Kantine)
        {
            _context.Kantines.Add(Kantine);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Kantine Kantine)
        {
            _context.Entry(Kantine).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var Kantine = await _context.Kantines.FindAsync(id);
            if (Kantine != null)
            {
                _context.Kantines.Remove(Kantine);
                await _context.SaveChangesAsync();
            }
        }
    }
}
