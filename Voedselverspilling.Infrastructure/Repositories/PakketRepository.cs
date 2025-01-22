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
    public class PakketRepository : IPakketRepository
    {
        private readonly ApplicationDbContext _context;

        public PakketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Pakket> GetByIdAsync(int id)
        {
            var pakket = await _context.Pakketten
                .Include(p => p.Producten)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(pakket == null)
            {
                throw new Exception("Pakket not found");
            }

            return pakket;
        }

        public async Task<IEnumerable<Pakket>> GetAllAsync()
        {
            return await _context.Pakketten.ToListAsync();
        }

        public async Task<Pakket> AddAsync(Pakket Pakket)
        {
            _context.Pakketten.Add(Pakket);
            await _context.SaveChangesAsync();
            return Pakket;
        }

        public async Task<Pakket> UpdateAsync(Pakket Pakket)
        {
            var pakketOld = await _context.Pakketten
                .Include(p => p.Producten)
                .FirstOrDefaultAsync(x => x.Id == Pakket.Id);
            if(pakketOld == null)
            {
                throw new Exception("Pakket not found");
            }
            if(pakketOld.ReservedBy != null)
            {
                throw new Exception("Pakket is already reserved");
            }
            _context.Entry(Pakket).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Pakket;
        }

        public async Task DeleteAsync(int id)
        {
            var Pakket = await _context.Pakketten
                .Include(p => p.Producten)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (Pakket == null)
            {
                throw new Exception("Pakket not found");
            }
            if (Pakket.ReservedBy != null)
            {
                throw new Exception("Pakket is already reserved");
            }
            _context.Pakketten.Remove(Pakket);
            await _context.SaveChangesAsync();
            
        }

        public async Task<IEnumerable<Pakket>> GetByEmailAsync(string eMail)
        {
            return await _context.Pakketten
                .Include(p => p.Producten)
                .Where(x => x.ReservedBy.Emailaddress == eMail && x.ReservedBy !=null)
                .ToListAsync();
        }

        public async Task<Pakket> ReservePakketAsync(int id, Student student)
        {
            var pakket = await _context.Pakketten
                .Include(p => p.Producten)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if(pakket == null)
            {
                throw new Exception("Pakket not found");
            }

            pakket.ReservedBy = student;
            _context.Entry(pakket).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return pakket;
        }
    }
}
