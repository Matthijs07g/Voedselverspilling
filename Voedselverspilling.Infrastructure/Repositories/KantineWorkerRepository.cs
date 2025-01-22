using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voedselverspilling.DomainServices.IRepositories;
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
            var kantineWorker = await _context.Medewerker.FindAsync(id);
            return kantineWorker ?? throw new KeyNotFoundException($"KantineWorker with id {id} not found");
        }

        public async Task<IEnumerable<KantineWorker>> GetAllAsync()
        {
            var kantineWorkers = await _context.Medewerker.ToListAsync();
            return kantineWorkers ?? throw new Exception("No kantine workers found");
        }

        public async Task<KantineWorker> AddAsync(KantineWorker kantineWorker)
        {
            _context.Medewerker.Add(kantineWorker);
            await _context.SaveChangesAsync();
            return kantineWorker;
        }

        public async Task<KantineWorker> UpdateAsync(KantineWorker kantineWorker)
        {
            _context.Entry(kantineWorker).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return kantineWorker;
        }

        public async Task DeleteAsync(int id)
        {
            var kantineWorker = await _context.Medewerker.FindAsync(id);
            if (kantineWorker == null)
            {
                throw new KeyNotFoundException($"KantineWorker with id {id} not found");
            }

            _context.Medewerker.Remove(kantineWorker);
            await _context.SaveChangesAsync();
        }
    }
}
