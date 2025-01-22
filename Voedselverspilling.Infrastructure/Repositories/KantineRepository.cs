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
    public class KantineRepository : IKantineRepository
    {
        private readonly ApplicationDbContext _context;

        public KantineRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Kantine> GetByIdAsync(int id)
        {
            var canteen = await _context.Kantines.FindAsync(id);
            return canteen ?? throw new Exception("Kantine not found");
        }

        public async Task<IEnumerable<Kantine>> GetAllAsync()
        {
            var canteens = await _context.Kantines.ToListAsync();
            return canteens ?? throw new Exception("No kantines found");
        }

        public async Task<Kantine> AddAsync(Kantine Kantine)
        {
            _context.Kantines.Add(Kantine);
            await _context.SaveChangesAsync();
            return Kantine;
        }

        public async Task<Kantine> UpdateAsync(Kantine Kantine)
        {
            var canteenOld = await _context.Kantines.FindAsync(Kantine.Id);
            if (canteenOld == null)
            {
                throw new Exception("Kantine not found");
            }

            _context.Entry(Kantine).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Kantine;
        }

        public async Task DeleteAsync(int id)
        {
            var Kantine = await _context.Kantines.FindAsync(id);
            if (Kantine == null)
            {
                throw new Exception("Kantine not found");
            }
            _context.Kantines.Remove(Kantine);
            await _context.SaveChangesAsync();
        }
    }
}
