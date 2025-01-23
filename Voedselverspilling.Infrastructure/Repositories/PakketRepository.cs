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
            Pakket.EindDatum = DateTime.Today.AddDays(2);
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

            if (pakket.Is18)
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var age = today.Year - student.GeboorteDatum.Year;

                // Adjust age if birthday hasn't occurred this year
                if (student.GeboorteDatum.Month > today.Month ||
                    (student.GeboorteDatum.Month == today.Month &&
                     student.GeboorteDatum.Day > today.Day))
                {
                    age--;
                }

                if (age < 18)
                {
                    throw new InvalidOperationException("Student must be 18 or older to reserve this package");
                }
            }

            if (pakket == null)
            {
                throw new Exception("Pakket not found");
            }

            var dateAvailability = await _context.Pakketten.Where(x => x.ReservedBy.Id == student.Id && x.ReserveringDatum == pakket.EindDatum)
                .ToListAsync();

            if(dateAvailability.Count > 0)
            {
                throw new InvalidOperationException("Already have a reservation for this date");
            }

                pakket.ReservedBy = student;
            _context.Entry(pakket).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return pakket;
        }
    }
}
