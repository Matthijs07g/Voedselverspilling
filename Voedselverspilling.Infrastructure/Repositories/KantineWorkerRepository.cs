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
    public class KantineWorkerRepository : IKantineWorkerRepository
    {
        private readonly ApplicationDbContext _context; 

        public KantineWorkerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<KantineWorker> GetByIdAsync(int id)
        {
            return await _context.Medewerker.FindAsync(id);
        }

        public async Task<IEnumerable<KantineWorker>> GetAllAsync()
        {
            return await _context.Medewerker.ToListAsync();
        }

        public async Task AddAsync(KantineWorker kantineWorker)
        {
            _context.Medewerker.Add(kantineWorker);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(KantineWorker kantineWorker)
        {
            _context.Entry(kantineWorker).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var kantineWorker = await _context.Medewerker.FindAsync(id);
            if (kantineWorker != null)
            {
                _context.Medewerker.Remove(kantineWorker);
                await _context.SaveChangesAsync();
            }
        }
    }
}
