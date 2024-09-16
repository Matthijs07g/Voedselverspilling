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
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Producten.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Producten.ToListAsync();
        }

        public async Task AddAsync(Product Product)
        {
            _context.Producten.Add(Product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product Product)
        {
            _context.Entry(Product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var Product = await _context.Producten.FindAsync(id);
            if (Product != null)
            {
                _context.Producten.Remove(Product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
